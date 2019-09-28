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
            new Timer(1000).Elapsed += (o, arg) => onPosition(Vehicle.Position);
        }
    }
}
