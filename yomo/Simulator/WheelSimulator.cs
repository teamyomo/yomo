using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yomo.Navigation;

namespace yomo.Simulator
{
    public class WheelSimulator : IWheel
    {
        public uint DutyRange => throw new NotImplementedException();

        public uint MaxDuty => throw new NotImplementedException();

        public double Speed => throw new NotImplementedException();

        public void SetSpeed(int speed)
        {
            throw new NotImplementedException();
        }
    }
}
