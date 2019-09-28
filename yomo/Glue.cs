using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using yomo.Navigation;

namespace yomo
{
    /// <summary>
    ///  Poor man's IoC container
    ///  Default... create the real wheel and position
    ///    when simulating, swap these out with simulated versions for testing
    /// </summary>
    static class Glue
    {
        public static Func<BcmPin, BcmPin, BcmPin, WheelId, IWheel> CreateWheel = (pwm,fwd,rev,id) => new Wheel(pwm,fwd,rev);
        public static Func<IPosition> CreatePosition = () => new Position();
   }
}
