using System;
using System.IO.Ports;
using System.Linq;

namespace yomo
{
	public class AttitudeInfo
	{
		public float AccX { get; set; }
		public float AccY { get; set; }
		public float AccZ { get; set; }
		public float Heading { get; set; }
		public float Pitch { get; set; }
		public float Roll { get; set; }
		public float Yaw { get; set; }
		public float Temp { get; set; }
	}

	public class Attitude : IDisposable
	{
		const string portName = "/dev/ttyUSB0";

		private SerialPort Serial_tty = new SerialPort();

		public Attitude()
		{
			Serial_tty.PortName = portName; //Assign the port name,
			Serial_tty.BaudRate = 57600;               // Baudrate = 9600bps
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

		static int ibuff = 0;
		static byte[] buffer = new byte[8];

		public void LoopReadPosition(Action<AttitudeInfo> onAttitude)
		{
			var start = DateTime.Now;
			int count = 0;
			var attitude = new AttitudeInfo ();

			for(int i = 0; i < 100000; i++)
			{
				if (0x55 != Serial_tty.ReadByte())
					continue;

				var cmd = Serial_tty.ReadByte();

				int checksum = 0x55 + cmd;
				ibuff = 0; // reset
				for(int j = 0; j < 8; j++)
				{
					checksum += (int)(buffer[j] = (byte)Serial_tty.ReadByte());
				}

				var recvCS = Serial_tty.ReadByte();
				if ((byte)(checksum & 0xFF) != recvCS)
					continue;

				switch(cmd)
				{
				case 0x51:
					var x = GetAcc(Serial_tty);
					var y = GetAcc(Serial_tty);
					var z = GetAcc(Serial_tty);

					attitude.AccX = x;
					attitude.AccY = y;
					attitude.AccZ = z;
					//                        Console.WriteLine($"X:{x}\tY:{y}\tZ:{z}\tG:{Math.Sqrt(x*x+y*y+z*z)}");
					count++;
					break;

				case 0x52:
					GetAngle(Serial_tty);
					GetAngle(Serial_tty);
					var hdg = GetAngle(Serial_tty);

					attitude.Heading = hdg;
					//                        Console.WriteLine($"X:{x}\tY:{y}\tZ:{z}\tG:{Math.Sqrt(x*x+y*y+z*z)}");
					count++;
					break;

				case 0x53:
					var pitch = -1.0f * (GetAngle (Serial_tty) - 90f);
					var roll = GetAngle (Serial_tty);
					var yaw = (-1.0F * GetAngle (Serial_tty) + 360 + 140) % 360;
					var temp = GetInt (Serial_tty) / 100.0;

					attitude.Pitch = pitch;
					attitude.Roll = roll;
					attitude.Yaw = yaw;
					attitude.Temp = (float)temp;

					count++;
					break;
				}

				if (count >= 3)
				{
					onAttitude (attitude);
					count = 0;
				}
			}
		}

		private static byte ReadByte()
		{
			if (ibuff > 8) return 0xFF;
			return buffer[ibuff++];
		}

		private static short GetInt(SerialPort sp)
		{
			return (short)((short)ReadByte() | (((short)ReadByte()) << 8));
		}

		private static short GetAngle(SerialPort sp)
		{
			return (short)((180 * GetInt(sp)) / 32767);
		}
		private static float GetAcc(SerialPort sp)
		{
			return (float)GetInt(sp) / 3891.08125f;
		}
	}
}
