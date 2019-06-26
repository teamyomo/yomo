using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yomo.Models;
using yomo.Utility;

namespace yomo.Command
{
     public class Grid
    {
        // Computes grids of veriuos types from a poly-region

        public void CreateGrid(Geometry geometry, float gridSpacing, float orientation)
        {
            var path = new List<Vector>();

            int len = geometry.Coordinates.Length;
            int len_2 = len / 2;
            var pts = new Vector[len_2];
            var slopes = new Vector[len_2];

            var ptMax = Vector.MinValue;
            var ptMin = Vector.MaxValue;

            var ptLast = new Vector(geometry.Coordinates[len-2], geometry.Coordinates[len - 1] );

            // Transform region float array into a point array, and derive the bounding box
            for (int i = 0; i < len; i += 2)
            {
                var iPt = i / 2;
                var pt = pts[iPt] = new Vector (geometry.Coordinates[i],geometry.Coordinates[i + 1] );
                slopes[iPt - 1] = pt - ptLast; // rise & runs for all lines
                ptLast = pt;

                // Find extents
                ptMax.Max(pt); 
                ptMin.Min(pt); 

                // Add the parimeter once, this give some space to turn
                path.Add(pt);
            }

            // Prepare some values we'll need to do gridding derived from the bounding box

            var regionBounds = ptMax - ptMin;
            
            // need to find a maximum bounds for count of grid lines... so assume it's perfectly diagonal and as far as it can be.
            var perpLines = (int)Math.Ceiling(regionBounds.Length/gridSpacing);

            var centroid = ptMin + (regionBounds/2);

            // Unit vector orientation
            // var uOrient = Vector.CreateVector(1.0, orientation);  I like dx/dy, but unit orientation might help
            var uOrient = Vector.Unit(orientation);

            // parallel slope is dy/dx, perpendicular slope is -dx/dy
            // use perp to find the next row, use par to find intercepts with boundry

            // Point closest to minimum will be the first point to start gridding
            var ptParallel = pts
                    .Select(p => new { p, dp = ptMin - p})
                    .OrderBy(p => p.dp.SquaredLength)
                    .First().p;

            // now figure out the step distance for each grid spacing, we'll use this to "walk" the parallel line points
            // (dx,dy) is a unit vector because it's a sin/cos pair, so this works without scaling effect.
            var deltaGrid = new Vector(-uOrient.Y * gridSpacing, uOrient.X * gridSpacing);

            // This is the working loop.
            // Go through all the line segments in the region and find all the intercepts with perpendicular lines at "gridSpacing" distances
            // ToDo: It may work cleaner to loop through the parallel routes, then the perimeter segments
            ptLast = pts[len_2 - 1];

            for (int i = 0; i < len_2; i++)
            {
                var pt = pts[i];
                var c = Vector.Determinent(uOrient, slopes[i]); 

                // TODO: if we flipped the j indexer with the i indexer, we wouldn't have to do this... 
                // and there may be some perimeter edge sorting tricks so it's faster
                var ptParT = ptParallel;

                if (Math.Abs(c) < 0.01)
                {
                    ptLast = pt;
                    continue; // parallel, no-mas
                }
                else
                {
                    var p3 = ptLast;
                    var p4 = pt;
                    var v34 = p4 - p3;

                    for (int j = 0; j < perpLines; j++)
                    {
                        // Find the x1,y1 x2,y2 for this parallel line 
                        ptParT += deltaGrid;

                        var p1 = ptParT;
                        var p2 = ptParT + uOrient;
                        var v12 = uOrient;

                        // Intersection determinent
                        var a = Vector.Determinent(p1, p2);
                        var b = Vector.Determinent(p3, p4);

                        // This is the solve for the missing points (intesection)
                        var intercept = (a * v34 - b * v12) / c;
                        
                        // Is this in segment formed by v3-v4 ?
                        if (Between(intercept.X, p3.X, p4.X) && Between(intercept.Y, p3.Y, p4.Y))
                        {
                            // We got it... so
                            // Add intercept (x,y) @ iPt for perpLine index
                        }
                    }

                    ptLast = pt;
                }
            }
        }

        /// <summary>
        ///  Returns true if the value v is between v0 and v1
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static bool Between(double v, double v0, double v1)
        {
            return ((v > v0 && v < v1) || (v > v1 && v < v0));
        }

        public enum CoastLineAlgorithm { FindFirst, FindLast}

        /// <summary>
        ///  Coast-lining algorithm; given closed polygon generates a "coastline" algorithm
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="rulerLength"></param>
        /// <returns></returns>
        public static double[] Coastline(double[] poly, float rulerLength, CoastLineAlgorithm algorithm)
        {
            List<double> coastal = new List<double>();
            var rulerLength_2 = rulerLength * rulerLength;
            int i = 0;

            while(i < poly.Length)
            {
                var x0 = poly[i];
                var y0 = poly[i + 1];
                double dx,dy;
                int j;
                bool found = false;


                if (algorithm == CoastLineAlgorithm.FindFirst) // longest coast line (faster computing and leaves more details, better for regions)
                {
                    j = i;
                    do
                    {
                        dx = (poly[++j] - x0);
                        dy = (poly[++j] - y0);
                    } while ((found = j < poly.Length) && dx * dx + dy * dy < rulerLength_2);
                }
                else // find last shortest coast line (slower computing, better for cleaning up routes)
                {
                    j = poly.Length;
                    do
                    {
                        dy = (poly[--j] - x0);
                        dx = (poly[--j] - y0);
                    } while ((found = j > i) && dx * dx + dy * dy > rulerLength_2);
                }

                if (found)
                {
                    var p0 = new Vector(x0, y0);
                    var p1 = new Vector(poly[j - 4], poly[j - 3]);
                    var p2 = new Vector(poly[j - 2], poly[j - 1]);

                    if (Vector.TryLineSegmentCircleInterpolate(p0, rulerLength, p1, p2, out p0))
                    {
                        coastal.Add(x0 = p0.X);
                        coastal.Add(y0 = p0.Y);
                    }
                }

                i = j;
            }

            return coastal.ToArray();
        }
    }
}
