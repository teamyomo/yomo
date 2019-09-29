using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using yomo.Navigation;

namespace yomo.Simulator
{
    public class PositionSimulator : IPosition
    {
        public void LoopReadPosition(Action<Position.PositionRecord> onPosition)
        {
            var tmr = new Timer(1000);
            tmr.Elapsed += (o, arg) => onPosition(Vehicle.Position);
            tmr.Start();
        }
    }
}
