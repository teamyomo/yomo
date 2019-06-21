using crozone.LinuxSerialPort;
using System;
using System.IO;
//using System.IO.Ports;
using System.Linq;

namespace yomo.Navigation
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

	public class Attitude
	{

		static int ibuff = 0;
		static byte[] buffer = new byte[8];

		public void LoopReadPosition(Action<AttitudeInfo> onAttitude)
		{
            const string portName = "/dev/ttyUSB0";
        
    		using (var Serial_tty = new LinuxSerialPort(portName) { BaudRate = 57600 })
            {
                Serial_tty.Open();

                Stream stream = Serial_tty.BaseStream;

                var start = DateTime.Now;
                int count = 0;
                var attitude = new AttitudeInfo();

                for (int i = 0; i < 100000; i++)
                {
                    if (0x55 != stream.ReadByte())
                        continue;

                    var cmd = stream.ReadByte();

                    int checksum = 0x55 + cmd;
                    ibuff = 0; // reset
                    for (int j = 0; j < 8; j++)
                    {
                        checksum += (int)(buffer[j] = (byte)stream.ReadByte());
                    }

                    var recvCS = stream.ReadByte();
                    if ((byte)(checksum & 0xFF) != recvCS)
                        continue;

                    switch (cmd)
                    {
                        case 0x51:
                            var x = GetAcc(stream);
                            var y = GetAcc(stream);
                            var z = GetAcc(stream);

                            attitude.AccX = x;
                            attitude.AccY = y;
                            attitude.AccZ = z;
                            //                        Console.WriteLine($"X:{x}\tY:{y}\tZ:{z}\tG:{Math.Sqrt(x*x+y*y+z*z)}");
                            count++;
                            break;

                        case 0x52:
                            GetAngle(stream);
                            GetAngle(stream);
                            var hdg = GetAngle(stream);

                            attitude.Heading = hdg;
                            //                        Console.WriteLine($"X:{x}\tY:{y}\tZ:{z}\tG:{Math.Sqrt(x*x+y*y+z*z)}");
                            count++;
                            break;

                        case 0x53:
                            var pitch = -1.0f * (GetAngle(stream) - 90f);
                            var roll = GetAngle(stream);
                            var yaw = (-1.0F * GetAngle(stream) + 360 + 140) % 360;
                            var temp = GetInt(stream) / 100.0;

                            attitude.Pitch = pitch;
                            attitude.Roll = roll;
                            attitude.Yaw = yaw;
                            attitude.Temp = (float)temp;

                            count++;
                            break;
                    }

                    if (count >= 3)
                    {
                        onAttitude(attitude);
                        count = 0;
                    }
                }
            }
        }



		private static byte ReadByte()
		{
			if (ibuff > 8) return 0xFF;
			return buffer[ibuff++];
		}

		private static short GetInt(Stream sp)
		{
			return (short)((short)ReadByte() | (((short)ReadByte()) << 8));
		}

		private static short GetAngle(Stream sp)
		{
			return (short)((180 * GetInt(sp)) / 32767);
		}
		private static float GetAcc(Stream sp)
		{
			return (float)GetInt(sp) / 3891.08125f;
		}
	}
}
