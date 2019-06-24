using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo.Utility
{
    public class PointF
    {
        public float X;
        public float Y;

        public static PointF operator +(PointF a, PointF b)
        {
            return new PointF { X = a.X + b.X, Y = a.Y + b.Y };
        }
        public static PointF operator -(PointF a, PointF b)
        {
            return new PointF { X = a.X - b.X, Y = a.Y - b.Y };
        }
        public static PointF operator *(float t, PointF a)
        {
            return new PointF { X = t * a.X, Y = t * a.Y};
        }
        public float MagSquared { get { return Dot(this, this); } }


        public float Distance(PointF fromPoint)
        {
            return (float)Math.Sqrt((fromPoint - this).MagSquared);
        }

        public static float Dot(PointF a, PointF b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }
}
