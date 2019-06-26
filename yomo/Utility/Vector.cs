using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo.Utility
{
    public class Vector
    {
        /// <summary>
        /// Static constant for the zero vector.
        /// </summary>
        public readonly static Vector Zero = new Vector(0, 0);

        /// <summary>
        /// Static constant for the unit vector.
        /// </summary>
        public readonly static Vector One = new Vector(1, 1);

        /// <summary>
        /// Static constant for the unit X vector.
        /// </summary>
        public readonly static Vector UnitX = new Vector(1, 0);

        /// <summary>
        /// Static constant for the unit Y vector.
        /// </summary>
        public readonly static Vector UnitY = new Vector(0, 1);

        /// <summary>
        ///  Minimum value vector
        /// </summary>
        public static readonly Vector MinValue = new Vector(double.MinValue, double.MinValue);

        /// <summary>
        ///  Maximum value vector
        /// </summary>
        public static readonly Vector MaxValue = new Vector(double.MaxValue, double.MaxValue);

        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// The length of the vector.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(SquaredLength);
            }
        }

        /// <summary>
        /// The squared length of the vector. Useful for optimalisation.
        /// </summary>
        public double SquaredLength
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        /// <summary>
        /// The absolute angle of the vector.
        /// </summary>
        public double Alpha
        {
            get
            {
                return Math.Atan2(Y, X);
            }
        }

        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="xValue">The x value of the vector. </param>
        /// <param name="yValue">The y value of the vector. </param>
        public Vector(double xValue, double yValue)         
        {
            X = xValue;
            Y = yValue;
        }

        /// <summary>
        /// Overrides the Equals method to provice better equality for vectors.
        /// </summary>
        /// <param name="obj">The object to test equality against.</param>
        /// <returns>Whether the objects are equal. </returns>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
                return false;

            Vector other = ((Vector)obj);
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Overrides the hashcode.
        /// </summary>
        /// <returns>The hashcode for the vector.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// ToString method overriden for easy printing/debugging.
        /// </summary>
        /// <returns>The string representation of the vector.</returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }


        /// <summary>
        ///  Set X or Y if the v1 X or Y are larger
        ///  used to find extents
        /// </summary>
        /// <param name="v1"></param>
        public void Max(Vector v1)
        {
            if (v1.X > X)
                X = v1.X;
            if (v1.Y > Y)
                Y = v1.Y;
        }

        /// <summary>
        ///  Set X or Y if the v1 X or Y are smaller
        ///  used to find extents
        /// </summary>
        /// <param name="v1"></param>
        public void Min(Vector v1)
        {
            if (v1.X < X)
                X = v1.X;
            if (v1.Y < Y)
                Y = v1.Y;
        }

        /*----------------------- Operator overloading below ------------------------------*/
        public static bool operator ==(Vector v1, Vector v2)
        {
            if (object.ReferenceEquals(v1, null))
            {
                return object.ReferenceEquals(v2, null);
            }
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.X * b.X, a.Y * b.Y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(double a, Vector b)
        {
            return new Vector(b.X * a, b.Y * a);
        }

        public static Vector operator /(Vector a, Vector b)
        {
            return new Vector(a.X / b.X, a.Y / b.Y);
        }

        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.X / b, a.Y / b);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }

        #region Static Utility Functions for Vectors

        /// <summary>
        /// Returns the dot product of the vectors.
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <returns>The dot product of the vectors. </returns>
        public static double Dot(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /// <summary>
        /// Returns the cross product of the vectors.
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <returns>The cross product of the vectors. </returns>
        public static double Cross(Vector v1, Vector v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /// <summary>
        /// Returns the angle between two vectors.
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <returns>The angle between two vectors. </returns>
        public static double Angle(Vector v1, Vector v2)
        {
            return Math.Acos(Dot(Normalize(v1), Normalize(v2)));
        }

        /// <summary>
        /// Returns the angle between a vector and the unitX vector.
        /// </summary>
        /// <param name="v1">The vector. </param>
        /// <returns>The angle between a vector and the unitX vector.. </returns>
        public static double Angle(Vector v1)
        {
            return Math.Acos(Dot(Normalize(v1), Normalize(Vector.UnitX)));
        }

        /// <summary>
        /// Normalises a vector.
        /// </summary>
        /// <param name="v">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector Normalize(Vector v)
        {
            if (v == Vector.Zero)
            {
                return Vector.Zero;
            }
            return v / v.Length;
        }

        /// <summary>
        /// Sets the length of the vector.
        /// </summary>
        /// <param name="v">The vector whose length to set.</param>
        /// <param name="length">The length of the vector.</param>
        /// <returns>The vector with correct length.</returns>
        public static Vector SetLength(Vector v, double length)
        {
            return Normalize(v) * length;
        }

        /// <summary>
        /// Rotate a vector by a certain angle.
        /// </summary>
        /// <param name="v">The vector to rotate. </param>
        /// <param name="angle">The angle to rotate with. </param>
        /// <returns>The rotated vector. </returns>
        public static Vector Rotate(Vector v, double angle)
        {
            double X = v.X * Math.Cos(angle) - v.Y * Math.Sin(angle);
            double Y = v.X * Math.Sin(angle) + v.Y * Math.Cos(angle);
            return new Vector(X, Y);
        }

        /// <summary>
        /// Rotate a vector to a certain angle.
        /// </summary>
        /// <param name="v">The vector to rotate. </param>
        /// <param name="angle">The angle to rotate to. </param>
        /// <returns>The rotated vector. </returns>
        public static Vector RotateTo(Vector v, double angle)
        {
            double length = v.Length;
            return CreateVector(length, angle);
        }

        /// <summary>
        /// Rotates the vector left by a 90 degrees turn.
        /// </summary>
        /// <param name="v">The vector to turn. </param>
        /// <returns>The rotated vector. </returns>
        public static Vector TurnLeft(Vector v)
        {
            return new Vector(-v.Y, v.X);
        }

        /// <summary>
        /// Rotates the vector right by a 90 degrees turn.
        /// </summary>
        /// <param name="v">The vector to turn. </param>
        /// <returns>The rotated vector. </returns>
        public static Vector TurnRight(Vector v)
        {
            return new Vector(v.Y, -v.X);
        }

        /// <summary>
        /// Determines the distance between the two vectors.
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <returns>The distance between the vectors.</returns>
        public static double Distance(Vector v1, Vector v2)
        {
            return (v1 - v2).Length;
        }

        /// <summary>
        /// Determines the squared distance between the two vectors.
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <returns>The squared distance between the vectors.</returns>
        public static double SquaredDistance(Vector v1, Vector v2)
        {
            return (v1 - v2).SquaredLength;
        }

        /// <summary>
        /// Interpolates linearly between two vectors. 
        /// </summary>
        /// <param name="v1">The first vector. </param>
        /// <param name="v2">The second vector. </param>
        /// <param name="fraction">The fraction to interpolate with. </param>
        /// <returns>The interpolated vector. </returns>
        public static Vector Lerp(Vector v1, Vector v2, double fraction)
        {
            return (1 - fraction) * v1 + fraction * v2;
        }

        /// <summary>
        /// Reflect a vector along a normal.
        /// </summary>
        /// <param name="v">The vector to reflect.</param>
        /// <param name="normal">The normal to reflect it along.</param>
        /// <returns>The reflected vector. </returns>
        public static Vector Reflect(Vector v, Vector normal)
        {
            Vector n = Normalize(normal);
            return 2 * (Dot(v, n)) * n - v;
        }

        /// <summary>
        /// Clamps the vector length between the two imput lengths (inclusive).
        /// </summary>
        /// <param name="v">The vector to clampl</param>
        /// <param name="min">The inclusive minimum length.</param>
        /// <param name="max">The inclusive maximum length.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector ClampLength(Vector v, double min, double max)
        {
            double vLength = v.Length;
            if (vLength < min)
            {
                return SetLength(v, min);
            }
            if (vLength > max)
            {
                return SetLength(v, max);
            }
            return v;
        }

        /// <summary>
        /// Create a vector from a length and angle.
        /// </summary>
        /// <param name="length">The length of the new vector. </param>
        /// <param name="angle">The angle of the new vector. </param>
        /// <returns>The new vector</returns>
        public static Vector CreateVector(double length, double angle)
        {
            return length * Unit(angle);
        }

        public static Vector Unit(double angle)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        /// <summary>
        /// Checks if the parameter vector is inside of the rectangle created by the other vectors.
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <param name="topLeft">The top left of the rectangle.</param>
        /// <param name="bottomRight">The bottom right of the rectangle.</param>
        /// <returns>True if inside</returns>
        public static bool InsideRectangle(Vector vector, Vector topLeft, Vector bottomRight)
        {
            bool horizontal = (vector.X < bottomRight.X) && (vector.X > topLeft.X);
            bool vertical = (vector.Y > bottomRight.Y) && (vector.Y < topLeft.Y);
            return horizontal && vertical;
        }

        /// <summary>
        ///  Returns the determinent of v1,v2
        ///  See 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Determinent(Vector v1, Vector v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }


        /// <summary>
        /// Interpolate the intersection of a line segment the arc of a circle
        /// 
        /// https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm/1084899#1084899
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool TryLineSegmentCircleInterpolate(Vector center, double radius, Vector v1, Vector v2, out Vector intercept)
        {
            var d = v2 - v1;
            var f = v1 - center;
            var r = radius;
            double a = d.SquaredLength;
            double b = 2 * Vector.Dot(f,d);
            double c = f.SquaredLength - r * r;
            intercept = Zero;

            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }
            else
            {
                // ray didn't totally miss sphere,
                // so there is a solution to
                // the equation.

                discriminant = Math.Sqrt(discriminant);

                // either solution may be on or off the ray so need to test both
                // t1 is always the smaller value, because BOTH discriminant and
                // a are nonnegative.
                double t1 = (-b - discriminant) / (2 * a);
                double t2 = (-b + discriminant) / (2 * a);

                // 3x HIT cases:
                //          -o->             --|-->  |            |  --|->
                // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

                // 3x MISS cases:
                //       ->  o                     o ->              | -> |
                // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

                // here t1 didn't intersect so we are started inside the sphere or completely past it.
                // For my use cases this ir the common scenarios
                if (t2 >= 0 && t2 <= 1)
                {
                    // ExitWound
                    intercept = t2 * v1 + (1 - t2) * v2;
                    return true;
                }

                // In this case we're outside the circle
                if (t1 >= 0 && t1 <= 1)
                {
                    // t1 is the intersection, and it's closer than t2
                    // (since t1 uses -b - discriminant)
                    // Impale, Poke
                    intercept = t1 * v1 + (1-t1)*v2;
                    return true;
                }

                // no intn: FallShort, Past, CompletelyInside
                return false;
            }
        }
        #endregion
    }
    
    /// <summary>
    /// Provides basic utilites for angles.
    /// </summary>
    public class AngleUtil
    {

        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static double ToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Converts an angle in radians to degrees.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static double ToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
