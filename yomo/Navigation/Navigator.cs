using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Native;
using Unosquare.WiringPi;

namespace yomo.Navigation
{
    /// <summary>
    ///  A Guild Navigator is a fictional humanoid in the Dune universe created by Frank Herbert. 
    ///  In this series and its derivative works, starships called heighliners employ a scientific 
    ///  phenomenon known as the Holtzman effect to "fold space" and thereby travel great distances 
    ///  across the universe instantaneously
    /// </summary>
	public class Navigator
	{
        // The wheels we intend to drive and which pin they are attached to.
        Wheel left = new Wheel(BcmPin.Gpio13);
        Wheel right = new Wheel(BcmPin.Gpio18);

        private PidController headingPid = new PidController(1.1, 0.1, 0.1, 180, -180);

        float idealHeading;
        float correctingHeading;
        float courseDeviation; // In meters (right positive, left negative)

        float distanceToTarget;
        float timeToTarget;

        float lastLat;
        float lastLng;
        float lastSpeed;
        float lastHeading;

        float targetLat;
        float targetLng;
        float targetSpeed = 0;

        public Navigator ()
		{
		}

        public void SetTarget(float lat, float lng, float speed)
        {
            targetLat = lat;
            targetLng = lng;
            targetSpeed = speed;
        }

        public void RefreshPosition(float lat, float lng, float speed, float heading)
        {
            lastLat = lat;
            lastLng = lng;
            lastSpeed = speed;
            lastHeading = heading;

            //CalculateDeviations();

            //PidControlHeading();

            //SteerWheels();
        }
	}

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

