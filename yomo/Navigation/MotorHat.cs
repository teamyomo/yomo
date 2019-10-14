using System;
using yomo.Utility;

namespace yomo.Navigation
{
    public class MotorHat : IWheel
    {
        class PWMConfig
        {
            public const int OFF = 0;
            public const int ON = 4095;

            public PWMConfig(byte Ch, byte E1, byte E2) { Channel = Ch; Enable1 = E1; Enable2 = E2; }
            public readonly byte Channel;     // PWM channel
            public readonly byte Enable1;     // Enable Pin 1
            public readonly byte Enable2;     // Enable Pin 2

            public static readonly PWMConfig[] Map = new PWMConfig[]
            {
               new PWMConfig( 8, 10,  9),
               new PWMConfig(13, 11, 12),
               new PWMConfig( 2,  4,  3),
               new PWMConfig( 7,  5,  6),
            };
        }

        /// <summary>
        ///  low duty cycles don't do anything, limit the range so it pulls down to "off"
        /// </summary>
        public uint MinDuty { get { return PWMConfig.ON/3; } }

        public uint MaxDuty { get { return PWMConfig.ON; } }

        int _speed = 0;

        static PCA9685 motorHat = new PCA9685(0x6F, 10000);
        
        public double Speed { get { return _speed; } }

        public int MotorNumber { get; set; }

        public void SetSpeed(int speed)
        {
            /// Get the pwm channels configuration
            var cfg = PWMConfig.Map[MotorNumber];

            var absSpeed = Math.Abs(speed);

            // Bound speed between min and max, same-signed
            var boundSpeed = (absSpeed < MinDuty) ? 0 : (int)Math.Sign(speed) * (int)Math.Min(absSpeed, MaxDuty);
            
            if (boundSpeed == 0)
            {
                Console.WriteLine("Stop");
                // Turns both enabled pins off
                motorHat.SetPwm(cfg.Enable1, PWMConfig.OFF, PWMConfig.ON);
                motorHat.SetPwm(cfg.Enable2, PWMConfig.OFF, PWMConfig.ON);
            }
            else if (_speed * boundSpeed <= 0) // switched direction?
            {
                Console.WriteLine(boundSpeed > 0 ? "Forward" : "Reverse");
                var on = boundSpeed > 0 ? PWMConfig.ON : PWMConfig.OFF;
                var off = PWMConfig.ON - on;

                motorHat.SetPwm(cfg.Enable1, on, off);
                motorHat.SetPwm(cfg.Enable2, off, on);
            }

            Console.WriteLine($"Setting Speed: {boundSpeed}");

            motorHat.SetPwmDuty(cfg.Channel, Math.Abs(_speed = boundSpeed));
        }
    }
}
