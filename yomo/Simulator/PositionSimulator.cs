using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yomo.Navigation;

namespace yomo.Simulator
{
    public class PositionSimulator : IPosition
    {
        public void LoopReadPosition(Action<Position.PositionRecord> onPosition)
        {
            throw new NotImplementedException();
        }
    }
}
