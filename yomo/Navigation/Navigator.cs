using System;
using yomo.Utility;
using static yomo.Navigation.Position;

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
        // Active Course
        Vector start;
        Vector target;

        double courseHeading = 0;
        double targetSpeed = 0;
        double targetHeading;
        double crossTrackError; // In meters (right positive, left negative)

        double distanceToTarget;
        double timeToTarget;

        // GPS inputs
        PositionRecord gps;

        Vector lastPosition { get { return gps.position; } }
        double lastSpeed { get { return gps.speed; } }
        double lastHeading { get { return gps.course; } }

        Kinematics kinematics = new Kinematics();
        Attitude attitude = new Attitude();
        IPosition position = yomo.Glue.CreatePosition();


        public Navigator ()
		{
            // start a background task to set the attitude loop
            // TODO: These attitude & position reader loops should run in the background

            // Get Heading
            // NOTE: I'm going to try not using the attitude sensor and rely on the accuracy of the GPS to drive heading changes
//            attitude.LoopReadPosition(att => lastHeading = att.Heading);

            // Get Position
            position.LoopReadPosition(g => gps = g);
        }

        /// <summary>
        ///  Do everything we need to navigate any given step
        /// </summary>
        public void NavigationLoop()
        {
            // Calculate cross track and heading
            CalculateCTEAndDistance();

            // Send to Kinematics
            kinematics.KinematicsLoop(targetHeading, lastHeading, targetSpeed, lastSpeed);
        }

        public void SetRoute(Vector beginning, Vector target, double speed)
        {
            start = beginning;
            this.target = target;
            targetSpeed = speed;

            var courseHeading = 180f * Math.Atan2(this.target.Y - start.Y, this.target.X - start.Y) / Math.PI;

        }

        public void SetTarget(Vector destinatin, double speed)
        {
            SetRoute(lastPosition, destinatin, speed);
        }

        /// <summary>
        /// This isn't some fancy cross track error that uses the arcs of the earth... it's basic trig.  how far is a point from a line and how far till I get there
        /// </summary>
        private void CalculateCTEAndDistance()
        {
            var delta = lastPosition - start;
            var l_2 = delta.SquaredLength;
            distanceToTarget = Math.Sqrt(l_2);

            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            // We clamp t from [0,1] to handle points outside the segment vw.

            var t = Math.Max(0, Math.Min(1, Vector.Dot(lastPosition - start, delta) / l_2));
            Vector projection = start + t * delta;  // Projection falls on the segment 
            crossTrackError = (lastPosition - projection).Length;

            var courseCorrection = Math.Min(30, 10 * crossTrackError); // correction angle is cross track error distance * factor, but not over 30 degrees 
            targetHeading = courseHeading + courseCorrection;

            timeToTarget = (lastSpeed > 0) ? distanceToTarget / lastSpeed : 999;
        }
    }
}

