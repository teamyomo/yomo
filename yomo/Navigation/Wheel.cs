﻿using System;
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
        int speed;
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

        public void SetSpeed(int speed)
        {
            var inRange = (uint)Math.Max(0, Math.Min(MaxSpeed, speed));
            pin.PwmRegister = (int)(MaxSpeed / inRange);
        }

        public uint MaxSpeed { get { return pin.PwmRange; } }
    }
}
