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
    public class Wheel
    {
        public double Speed { get; private set; }

        GpioPin pin;

        public Wheel(BcmPin gpioPin)
        {
            // TODO: Check out:
            // https://raspberrypi.stackexchange.com/questions/4906/control-hardware-pwm-frequency
            // https://stackoverflow.com/questions/20081286/controlling-a-servo-with-raspberry-pi-using-the-hardware-pwm-with-wiringpi

            pin = (GpioPin)Pi.Gpio[gpioPin];
            pin.PinMode = GpioPinDriveMode.PwmOutput;
            pin.PwmMode = PwmMode.MarkSign;

            // pwmFrequency in Hz = 19.2e6 Hz / pwmClock / pwmRange.
            pin.PwmClockDivisor = 16; // 1 is 4096, possible values are all powers of 2 starting from 2 to 2048
            pin.PwmRange = 1024;
            pin.PwmRegister = 0; // (int)(pin.PwmRange * decimal duty); // This goes from 0 to PwmRange-1
        }

        const double kFactor = 100.0; // convertion from requested speed to PWM duty
        const uint MinDuty = 500;

        /// <summary>
        ///  Let's say that's M/S
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeed(double speed)
        {
            Speed = speed;
            var pwmDuty = (uint)(speed * kFactor);
            var inRange = (uint)Math.Max(MinDuty, Math.Min(MaxDuty, pwmDuty));
            pin.PwmRegister = (int)(MaxDuty / inRange);
        }

        public uint MaxDuty { get { return pin.PwmRange; } }
    }
}
