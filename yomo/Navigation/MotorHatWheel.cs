using System;
using yomo.Utility;

namespace yomo.Navigation
{
    public class MotorHatWheel : IWheel
    {
        public uint DutyRange { get { return 1024; } }

        public uint MaxDuty { get { return 1024; } }

        int _speed = 0;

        static PCA9685 motorHat = new PCA9685();
        
        public double Speed { get { return _speed; } }

        public int Channel { get; set; }

        public void SetSpeed(int speed)
        {
            motorHat.SetPwmDuty(Channel, _speed = speed);
        }
    }
}
