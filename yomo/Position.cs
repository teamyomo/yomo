
using System;
using System.IO.Ports;

namespace yomo
{
	public class Position : IDisposable	
	{
		const string portName = "/dev/serial0";

		private SerialPort Serial_tty = new SerialPort();

		public Position()
		{

			Serial_tty.PortName = portName; //Assign the port name,
			Serial_tty.BaudRate = 9600;               // Baudrate = 9600bps
			try
			{
				Serial_tty.Open();                 // Open the port
			}
			catch (Exception x)
			{
				throw new Exception($"Error opening serial port: {Serial_tty.PortName}", x);
			}
		}

		public void Dispose()
		{
			Serial_tty.Close();                // Close port
		}


		public void LoopReadPosition(Action<float,float> onPosition)
		{
			// float latAvg = 0f;
			// float lngAvg = 0f;
			// float weight = 0f;

			for(int i = 0; i < 1000; i++)
			{
				var lin = Serial_tty.ReadLine();

				var parts = lin.Split(',');	    
				if (!lin.StartsWith("$GPGGA") ||
					parts[6] == "0")
					continue;

				//            var todUTC = parts[1];
				var lat = float.Parse(parts[2]);
				var deg = (float)Math.Floor(lat/100);
				lat = deg + (lat-(100f*deg))/60f;

				if (parts[3]!="N")
					lat = -lat;

				var lng = float.Parse(parts[4]);
				deg = (float)Math.Floor(lng/100f);
				lng = deg + (lng - (100f*deg))/60f;

				if (parts[5]!="E")
					lng = -lng;

//				var hdop = float.Parse(parts[6]);

				// latAvg += hdop*lat;
				// lngAvg += hdop*lng;
				// weight += hdop;

				onPosition (lat, lng);
			}
		}
	}
}

