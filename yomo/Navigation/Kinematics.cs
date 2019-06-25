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
        Wheel left = new Wheel(BcmPin.Gpio13);
        Wheel right = new Wheel(BcmPin.Gpio18);

        DateTime last = DateTime.Now;

        float headingLast = 0;

        const float kAlpha = 1.0f; // steering coefficient, more steer harder, less steer gently

        private PidController headingPid = new PidController(1.1, 0.1, 0.1, 180, -180);
        private PidController speedPid = new PidController(1.1, 0.1, 0.1, 180, -180);

        public void KinematicsLoop(float headingDesired, float headingActual, float speedDesired, float speedActual)
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
            headingLast = (float)heading;

            // Electronically "Mix" the speed and angle angle change to set the left/right motor speeds
            left.SetSpeed((float)(speed + dAlpha));
            right.SetSpeed((float)(speed - dAlpha));
        }
    }
}
