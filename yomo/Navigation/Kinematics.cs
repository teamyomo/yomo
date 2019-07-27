using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;

namespace yomo.Navigation
{
    /// <summary>
    /// Manage the PID loop that controls the wheels
    /// </summary>
    public class Kinematics
    {
        // The wheels we intend to drive and which pin they are attached to.
        Wheel left = new Wheel(BcmPin.Gpio13, BcmPin.Gpio06, BcmPin.Gpio05);
        Wheel right = new Wheel(BcmPin.Gpio18, BcmPin.Gpio23, BcmPin.Gpio24);

        DateTime last = DateTime.Now;

        double headingLast = 0;

        const double kAlpha = 1.0f; // steering coefficient, more steer harder, less steer gently

        private PidController headingPid = new PidController(1.1, 0.1, 0.1, 180, -180);
        private PidController speedPid = new PidController(1.1, 0.1, 0.1, 180, -180);

        public void KinematicsLoop(double headingDesired, double headingActual, double speedDesired, double speedActual)
        {
            // Set the heading & speed PID controllers
            headingPid.SetPoint = headingDesired;
            headingPid.ProcessVariable = headingActual;

            speedPid.SetPoint = speedDesired;
            speedPid.ProcessVariable = speedActual;

            // Delta time
            var now = DateTime.Now;
            var dt = now - last;
            last = now;

            // Run the controller
            var heading = headingPid.ControlVariable(dt);
            var speed = headingPid.ControlVariable(dt);

            // calculate the change in angle from last
            var dAlpha = kAlpha * (headingLast - heading);
            headingLast = heading;

            // Electronically "Mix" the speed and angle angle change to set the left/right motor speeds
            left.SetSpeed((int)(speed + dAlpha));
            right.SetSpeed((int)(speed - dAlpha));
        }
    }
}
