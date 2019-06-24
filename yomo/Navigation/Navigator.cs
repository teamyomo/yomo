using System;
using yomo.Utility;

namespace yomo.Navigation
{
    /// <summary>
    ///  A Guild Navigator is a fictional humanoid in the Dune universe created by Frank Herbert. 
    ///  In this series and its derivative works, starships called heighliners employ a scientific 
    ///  phenomenon known as the Holtzman effect to "fold space" and thereby travel great distances 
    ///  across the universe instantaneously
    /// </summary>
	public class Navigator
	{
        float idealHeading;
        float correctingHeading;
        float courseDeviation; // In meters (right positive, left negative)

        float distanceToTarget;
        float timeToTarget;

        float lastLat;
        float lastLng;
        float lastSpeed;
        float lastHeading;

        float targetLat;
        float targetLng;
        float targetSpeed = 0;

        public Navigator ()
		{
		}

        public void SetTarget(float lat, float lng, float speed)
        {
            targetLat = lat;
            targetLng = lng;
            targetSpeed = speed;
        }

        public void RefreshPosition(float lat, float lng, float speed, float heading)
        {
            lastLat = lat;
            lastLng = lng;
            lastSpeed = speed;
            lastHeading = heading;

            //CalculateDeviations();

            //PidControlHeading();

            //SteerWheels();
        }

        /// <summary>
        /// This isn't some fancy cross track error that uses the arcs of the earth... it's basic trig.  how far is a point from a line and how far till I get there
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="position"></param>
        /// <param name="crossTrackError"></param>
        /// <param name="distance"></param>
        public static void CalculateCTEAndDistance(PointF start, PointF end, PointF position, out double crossTrackError, out double distance)
        {
            var delta = end - start;
            var l_2 = delta.MagSquared;
            distance = Math.Sqrt(l_2);

            if (Math.Abs(l_2) < 0.01)
            {
                distance = crossTrackError = position.Distance(start);   // v == w case
                return;
            }

            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            // We clamp t from [0,1] to handle points outside the segment vw.

            float t = Math.Max(0, Math.Min(1, PointF.Dot(position - start, delta) / l_2));
            PointF projection = start + t * delta;  // Projection falls on the segment 
            crossTrackError = position.Distance(projection);
        }
    }
}

