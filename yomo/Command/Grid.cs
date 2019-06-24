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
            var path = new List<PointF>();

            int len = geometry.Coordinates.Length;
            int len_2 = len / 2;
            var pts = new PointF[len_2];
            var slopes = new PointF[len_2];

            var ptMax = new PointF { X = float.MinValue, Y = float.MinValue };
            var ptMin = new PointF { X = float.MaxValue, Y = float.MaxValue };

            var ptLast = new PointF { X = geometry.Coordinates[len-2], Y = geometry.Coordinates[len - 1] };

            for (int i = 0; i < len; i += 2)
            {
                var iPt = i / 2;
                var pt = pts[iPt] = new PointF { X = geometry.Coordinates[i], Y = geometry.Coordinates[i + 1] };
                slopes[iPt - 1] = new PointF { X = pt.X - ptLast.X, Y = pt.Y - ptLast.Y }; // rise & runs for all lines
                ptLast = pt;

                // Find extents
                ptMax.X = Math.Max(ptMax.X, pt.X);
                ptMax.Y = Math.Max(ptMax.Y, pt.Y);
                ptMin.X = Math.Min(ptMax.X, pt.X);
                ptMin.Y = Math.Min(ptMax.X, pt.Y);

                // Add the parimeter once, this give some space to turn
                path.Add(pt);
            }

            var bndsDX = (ptMax.X - ptMin.X);
            var bndsDY = (ptMax.Y - ptMin.Y);
            
            // need to find a maximum bounds for count of grid lines... so assume it's perfectly diagonal and as far as it can be.
            var perpLines = Math.Ceiling(Math.Sqrt(bndsDX * bndsDX + bndsDY * bndsDY) / gridSpacing);

            var ptCtr = new PointF { X = ptMin.X + bndsDX / 2, Y = ptMin.Y + bndsDX / 2 };

            var dx = Math.Sin(orientation); // run
            var dy = Math.Cos(orientation); // rise

            // parallel slope is dy/dx, perpendicular slope is -dx/dy
            // use perp to find the next row, use par to find intercepts with boundry

            // Point closest to minimum will be the first point to start gridding
            var ptParallel = pts
                    .Select(p => new {p, dx = p.X - ptMin.X, dy = p.Y - ptMin.Y })
                    .OrderBy(p => p.dx * p.dx + p.dy + p.dy)
                    .First().p;

            // now figure out the step distance for each grid spacing, we'll use this to "walk" the parallel line points
            // (dx,dy) is a unit vector because it's a sin/cos pair, so this works without scaling effect.
            var dxGrid = (float)(-dy * gridSpacing);
            var dyGrid = (float)(dx * gridSpacing);

            ptLast = pts[len_2 - 1];
            for (int i = 0; i < len_2; i++)
            {
                var pt = pts[i];
                var x34 = slopes[i].X;
                var y34 = slopes[i].Y;

                var c = (float)(dx * y34 - dy * x34);

                if (Math.Abs(c) < 0.01)
                {
                    ptLast = pt;
                    continue; // parallel, no-mas
                }
                else
                {
                    for (int j = 0; j < perpLines; j++)
                    {
                        // Find the x1,y1 x2,y2 for this parallel line 
                        ptParallel.X += dxGrid;
                        ptParallel.Y += dyGrid;

                        var x1 = ptParallel.X;
                        var y1 = ptParallel.Y;
                        var x2 = (float)(x1 + dx);   // we "make up" a point on the same line by following the slope of the grid angle
                        var y2 = (float)(y1 + dy);

                        // TODO: There's like optimization that can be done here as we have the slopes of |(3,4)
                        var x3 = ptLast.X;  // these (3,4) are just the last-current point which makes a line segment
                        var y3 = ptLast.Y;
                        var x4 = pt.X;
                        var y4 = pt.Y;

                        var x12 = x2 - x1;
                        var y12 = y2 - y1;

                        // Intersection determinent
                        float a = x1 * y2 - y1 * x2;
                        float b = x3 * y4 - y3 * x4;

                        // this is the solve for the missing points (intesection)
                        float x = (a * x34 - b * x12) / c;
                        float y = (a * y34 - b * y12) / c;

                        // Is this in segment |3-4 ?
                        if (Between(x, x3, x4) && Between(y, y3, y4))
                        {
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
        public static bool Between(float v, float v0, float v1)
        {
            return Math.Abs(v - v0) + Math.Abs(v-v1) < 0.01;
        }
    }
}
