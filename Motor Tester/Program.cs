using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Peripherals;
using Unosquare.WiringPi;
using yomo.Navigation;

namespace Motor_Tester
{
    class Program
    {
//        static Wheel left = new Wheel(BcmPin.Gpio13, BcmPin.Gpio06, BcmPin.Gpio05);
        static Wheel right = new Wheel(BcmPin.Gpio18, BcmPin.Gpio23, BcmPin.Gpio24);
        static ADS1015 adcDevice = new ADS1015() {Gain = ADS1x15.AdsGaint.GAINONE};

        static void Main(string[] args)
        {
            Console.WriteLine("Motor Tester");

            Pi.Init<BootstrapWiringPi>();

            DumpVA();

            int speed = 0;
            int inc = 100;

            for (int i = 0; i < 30000; i++)
            {
                System.Threading.Thread.Sleep(50);
                DumpVA();
            }

            Console.WriteLine("All Stop!");
            right.SetSpeed(0);
            System.Threading.Thread.Sleep(2000);
            DumpVA();

            Console.WriteLine("Half");
            right.SetSpeed((int)right.MaxDuty/2);
            System.Threading.Thread.Sleep(1000);
            DumpVA();

            Console.WriteLine("Full");
            right.SetSpeed((int)right.MaxDuty);
            System.Threading.Thread.Sleep(10000);
            DumpVA();

            Console.WriteLine("Half");
            right.SetSpeed((int)right.MaxDuty / 2);
            System.Threading.Thread.Sleep(1000);
            DumpVA();

            Console.WriteLine("All Stop!");
            right.SetSpeed(0);
            DumpVA();

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(500);
                DumpVA();

                speed += inc;
                if (speed > right.DutyRange || speed < -right.DutyRange)
                {
                    inc = -inc;
                    speed += inc;
                }

                Console.WriteLine($"Speed {speed}");
                right.SetSpeed(speed);
            }

            right.SetSpeed(0);
            DumpVA();
        }

        private static void DumpVA()
        {
            var v = adcDevice.ReadChannel(0);
            var a = adcDevice.ReadChannel(2);
            Console.WriteLine($"V:{v.ToString("X4")}\tA:{a.ToString("X4")}");
        }
    }
}
