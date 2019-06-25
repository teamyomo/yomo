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
        PointF initial;
        PointF target;
        float courseHeading;
        float targetSpeed = 0;

        float distanceToTarget;
        float timeToTarget;

        PointF lastPosition;

        float lastSpeed;
        float lastHeading;

        float crossTrackError; // In meters (right positive, left negative)
        float targetHeading;

        Kinematics kinematics = new Kinematics();
        Attitude attitude = new Attitude();
        Position position = new Position();

        public Navigator ()
		{
            // start a background task to set the attitude loop
		}

        /// <summary>
        ///  Do everything we need to navigate any given step
        /// </summary>
        public void NavigationLoop()
        {
            // TODO: These attitude & position reader loops should run in the background

            // Get Heading
            attitude.LoopReadPosition(att => lastHeading = att.Heading);

            // Get Position
            position.LoopReadPosition(pos => { lastPosition.Y = pos.lat; lastPosition.X = pos.lng; lastSpeed = pos.speed; });

            // Do the math to navigate

            // Calculate cross track and heading
            CalculateCTEAndDistance();

            // Send to Kinematics
            kinematics.KinematicsLoop(targetHeading, lastHeading, targetSpeed, lastSpeed);
        }

        public void SetRoute(PointF beginning, PointF target, float speed)
        {
            initial = beginning;
            this.target = target;
            targetSpeed = speed;

            var courseHeading = 180f * Math.Atan2(this.target.Y - initial.Y, this.target.X - initial.Y) / Math.PI;

        }

        public void SetTarget(PointF destinatin, float speed)
        {
            SetRoute(lastPosition, destinatin, speed);
        }

        /// <summary>
        /// This isn't some fancy cross track error that uses the arcs of the earth... it's basic trig.  how far is a point from a line and how far till I get there
        /// </summary>
        private void CalculateCTEAndDistance()
        {
            var delta = lastPosition - initial;
            var l_2 = delta.MagSquared;
            distanceToTarget = (float)Math.Sqrt(l_2);

            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            // We clamp t from [0,1] to handle points outside the segment vw.

            float t = Math.Max(0, Math.Min(1, PointF.Dot(lastPosition - initial, delta) / l_2));
            PointF projection = initial + t * delta;  // Projection falls on the segment 
            crossTrackError = lastPosition.Distance(projection);

            var courseCorrection = Math.Min(30, 10 * crossTrackError); // correction angle is cross track error distance * factor, but not over 30 degrees 
            targetHeading = courseHeading + courseCorrection;

            timeToTarget = (lastSpeed > 0) ? distanceToTarget / lastSpeed : 999;
        }
    }
}

