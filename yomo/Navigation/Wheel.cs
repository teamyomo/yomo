using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Native;
using Unosquare.WiringPi;

namespace yomo.Navigation
{
    public class Wheel : IWheel
    {
        public double Speed { get; private set; }

        GpioPin PinPwm;
        GpioPin PinFwd;
        GpioPin PinRev;

        /// <summary>
        ///  Setup the wheel
        /// </summary>
        /// <param name="pinPwm">Pin used for the PWM</param>
        /// <param name="pinFwd">Pin used to enable forward</param>
        /// <param name="pinRev">Pin used to enable reverse</param>
        public Wheel(BcmPin pinPwm, BcmPin pinFwd, BcmPin pinRev)
        {

            PinFwd = (GpioPin)Pi.Gpio[pinFwd];
            PinFwd.PinMode = GpioPinDriveMode.Output;
            PinFwd.Write(true);

            PinRev = (GpioPin)Pi.Gpio[pinRev];
            PinRev.PinMode = GpioPinDriveMode.Output;
            PinRev.Write(true);

            // TODO: Check out:
            // https://raspberrypi.stackexchange.com/questions/4906/control-hardware-pwm-frequency
            // https://stackoverflow.com/questions/20081286/controlling-a-servo-with-raspberry-pi-using-the-hardware-pwm-with-wiringpi

            PinPwm = (GpioPin)Pi.Gpio[pinPwm];
            PinPwm.PinMode = GpioPinDriveMode.PwmOutput;
            PinPwm.PwmMode = PwmMode.MarkSign;

            // pwmFrequency in Hz = 19.2e6 Hz / pwmClock / pwmRange.
            PinPwm.PwmClockDivisor = 16; // 1 is 4096, possible values are all powers of 2 starting from 2 to 2048
            PinPwm.PwmRange = 1024;
            PinPwm.PwmRegister = 0; // (int)(pin.PwmRange * decimal duty); // This goes from 0 to PwmRange-1

        }

        const double kFactor = 100.0; // convertion from requested speed to PWM duty
        const uint MinDuty = 200;

        /// <summary>
        ///  Let's say that's M/S
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeed(int speed)
        {
            Speed = speed;
            var pwmDuty = MinDuty + (uint)Math.Abs(speed);
            var inRange = (uint)Math.Min(MaxDuty, pwmDuty);
            PinPwm.PwmRegister = (int)inRange;

            var fwd = speed > 0;
            var directionChanged = PinFwd.Value != fwd || PinRev.Value == fwd;

            if (directionChanged || speed == 0)
            {
                // Turn both off... can't have both on
                PinFwd.Write(false);
                PinRev.Write(false);

                if (speed != 0)
                {
                    // set as appropriate
                    PinFwd.Write(fwd);
                    PinRev.Write(!fwd);
                }
            }
        }

        public uint DutyRange { get { return MaxDuty - MinDuty; } }
        public uint MaxDuty { get { return PinPwm.PwmRange; } }
    }
}
