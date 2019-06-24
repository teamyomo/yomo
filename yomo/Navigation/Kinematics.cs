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

        private PidController headingPid = new PidController(1.1, 0.1, 0.1, 180, -180);
    }
}
