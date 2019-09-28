using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yomo.Navigation;
using yomo.Utility;

namespace yomo.Simulator
{
    public class Vehicle
    {
        // Constant that's used to derive how much motor differential changes heading on each step
        const double kHeading = 0.05f;

        public static double LeftWheelSpeed { get; set; }
        public static double RightWheelSpeed { get; set; }

        public static DateTime LastReq { get; set; }

        public static double Heading { get; set; }

        public static Position.PositionRecord lastPosition { get; set; }

        static Vehicle()
        {
            LastReq = DateTime.UtcNow;
        }

        public static Position.PositionRecord Position
        {
            get
            {
                // calculate the time lapse
                var now = DateTime.UtcNow;
                var dt = now.Subtract(LastReq).TotalMilliseconds;


                // calculate the distance
                var distance = (LeftWheelSpeed + RightWheelSpeed) * dt / 2;
                var changeInHeading = kHeading * (LeftWheelSpeed - RightWheelSpeed) * dt;

                // Wrap around 0..2Pi
                Heading = (Heading + changeInHeading + 2 * Math.PI) % (2 * Math.PI);

                lastPosition.position += new Vector(Math.Cos(Heading) * distance, Math.Sin(Heading) * distance);

                return lastPosition;
            }
        }

    }
}
