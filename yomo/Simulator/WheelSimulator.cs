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

        public WheelId Id { get; set; }

        public WheelSimulator(WheelId id)
        {
            Id = id;
        }

        public void SetSpeed(int speed)
        {
            if (Id == WheelId.LeftRear)
            {
                Vehicle.LeftWheelSpeed = speed;
            }
            else
            {
                Vehicle.RightWheelSpeed = speed;
            }
        }
    }
}
