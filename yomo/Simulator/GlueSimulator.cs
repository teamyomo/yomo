using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo.Simulator
{
    public static class SimulatorGlue
    {
        public static void Glue()
        {
            yomo.Glue.CreateWheel = (pwm, fwd, rev, id) => new WheelSimulator(id);
            yomo.Glue.CreatePosition = () => new PositionSimulator();
        }
    }
}
