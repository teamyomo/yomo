using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using yomo.Navigation;

namespace yomo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Initialize Pi");
            Pi.Init<BootstrapWiringPi>();

            Console.WriteLine("Glueing Dependencies");

            // Setup the testbot glue
            if (args.Any(a => a.Contains("--TestBot")))
                Glue.TestBot();
            else if (args.Any(a => a.Contains("--SoftBot")))
                Glue.Simulate();
            else
                Glue.YomoBot();

            if (args.Any(a => a.Contains("--WheelTest")))
                WheelTest();

            CreateWebHostBuilder(args).Build().Run();
        }

        private static void WheelTest()
        {
            Console.WriteLine("Creating test wheels");

            var lf = Glue.CreateWheel(BcmPin.Gpio00, BcmPin.Gpio01, BcmPin.Gpio02, WheelId.RightRear);
            var rf = Glue.CreateWheel(BcmPin.Gpio00, BcmPin.Gpio01, BcmPin.Gpio02, WheelId.RightFront);

            Go(lf, rf, 2048, 2048, 2000);
            Go(lf, rf, 2048, -2048, 1000);
            Go(lf, rf, 2048, 2048, 2000);
            Go(lf, rf, 1800, 3120, 3000);
            Go(lf, rf, 0, 0, 0);
        }

        private static void Go(IWheel lf, IWheel rf, int l, int r, int duration)
        {
            lf.SetSpeed(l);
            rf.SetSpeed(r);
            System.Threading.Thread.Sleep(duration);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0")
                ;
    }
}
