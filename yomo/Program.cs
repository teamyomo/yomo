using System;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;

namespace yomo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("YoMo lawn mowing software");

            Console.WriteLine("Initializing GPIO");
            Pi.Init<BootstrapWiringPi>();

            Console.WriteLine("Starting sensor listeners");
            var position = new Position();
			Task.Run(() => position.LoopReadPosition((lat,lng)=>
				{
					Console.WriteLine($"Pos: {lat},{lng}");
				}));

			var attitude = new Attitude();
			Task.Run(() => attitude.LoopReadPosition(att=>
				{
					Console.WriteLine($"Hdg: {att.Heading}");
				}));

            var locomotion = new Locomotion();

			// Drive the wheels towards a target

            // Do we have a command?

			// take a nap
			for(;;) System.Threading.Thread.Sleep(int.MaxValue);
		}
	}
}
