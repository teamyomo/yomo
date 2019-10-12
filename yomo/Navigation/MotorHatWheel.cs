using System;
using yomo.Utility;

namespace yomo.Navigation
{
    public class MotorHatWheel : IWheel
    {
        class PWMConfig
        {
            public const int OFF = 0;
            public const int ON = 4096;

            public PWMConfig(byte Ch, byte E1, byte E2) { Channel = Ch; Enable1 = E1; Enable2 = E2; }
            public readonly byte Channel;     // PWM channel
            public readonly byte Enable1;     // Enable Pin 1
            public readonly byte Enable2;     // Enable Pin 2

            public static readonly PWMConfig[] Map = new PWMConfig[]
            {
               new PWMConfig(8, 9, 10),
               new PWMConfig(13, 12, 11),
               new PWMConfig(2, 3, 4),
               new PWMConfig(7, 6, 5),
            };
        }

        /// <summary>
        ///  low duty cycles don't do anything, limit the range so it pulls down to "off"
        /// </summary>
        public uint DutyRange { get { return 2*PWMConfig.ON/3; } }

        public uint MaxDuty { get { return PWMConfig.ON; } }

        int _speed = 0;

        static PCA9685 motorHat = new PCA9685();
        
        public double Speed { get { return _speed; } }

        public int MotorNumber { get; set; }

        public void SetSpeed(int speed)
        {
            /// Get the pwm channels configuration
            var cfg = PWMConfig.Map[MotorNumber];

            var absSpeed = Math.Abs(speed);

            // Bound speed between min and max, same-signed
            var boundSpeed = (absSpeed < MaxDuty - DutyRange) ? 0 : Math.Sign(speed) * Math.Min(absSpeed, (int)MaxDuty);

            if (boundSpeed == 0)
            {
                // Turns both enabled pins off
                motorHat.SetPwm(cfg.Enable1, PWMConfig.OFF, PWMConfig.ON);
                motorHat.SetPwm(cfg.Enable2, PWMConfig.OFF, PWMConfig.ON);
            }
            else if (_speed * boundSpeed < 0) // switched direction?
            {
                var on = boundSpeed > 0 ? PWMConfig.ON : PWMConfig.OFF;
                var off = PWMConfig.ON - on;

                motorHat.SetPwm(cfg.Enable1, on, off);
                motorHat.SetPwm(cfg.Enable2, off, on);
            }

            motorHat.SetPwmDuty(cfg.Channel, _speed = boundSpeed);
        }
    }
}
