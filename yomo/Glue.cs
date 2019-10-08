using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using yomo.Navigation;
using yomo.Simulator;

namespace yomo
{
    /// <summary>
    ///  Poor man's IoC container
    ///  Default... create the real wheel and position
    ///    when simulating, swap these out with simulated versions for testing
    /// </summary>
    public static class Glue
    {
        public static Func<BcmPin, BcmPin, BcmPin, WheelId, IWheel> CreateWheel;
        public static Func<IPosition> CreatePosition;

        public static void Simulate()
        {
            yomo.Glue.CreateWheel = (pwm, fwd, rev, id) => new WheelSimulator(id);
            yomo.Glue.CreatePosition = () => new PositionSimulator();
        }

        public static void YomoBot()
        {
            yomo.Glue.CreateWheel = (pwm, fwd, rev, id) => new Wheel(pwm, fwd, rev);
            yomo.Glue.CreatePosition = () => new Position();
        }

        public static void TestBot()
        {
            yomo.Glue.CreateWheel = (pwm, fwd, rev, id) => new MotorHatWheel() { Channel = (int)(id - 1) };
            yomo.Glue.CreatePosition = () => new Position();
        }
    }
}
