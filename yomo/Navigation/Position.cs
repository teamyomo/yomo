
using crozone.LinuxSerialPort;
using System;
using System.IO;
using System.IO.Ports;

namespace yomo.Navigation
{
	public class Position
	{
		const string portName = "/dev/serial0";

        public class PositionRecord
        {
            public float lat;
            public float lng;
            public float speed;
        }

        public void LoopReadPosition(Action<PositionRecord> onPosition)
		{
            // float latAvg = 0f;
            // float lngAvg = 0f;
            // float weight = 0f;

            using (var Serial_tty = new LinuxSerialPort(portName) { BaudRate = 9600 })
            {
                Serial_tty.Open();

                var stream = new StreamReader(Serial_tty.BaseStream);

                for (int i = 0; i < 1000; i++)
                {
                    var lin = stream.ReadLine();

                    var parts = lin.Split(',');
                    if (!lin.StartsWith("$GPRMC") ||
                        parts[2] != "A")
                        continue;

                    //            var todUTC = parts[1];
                    var lat = float.Parse(parts[3]);
                    var deg = (float)Math.Floor(lat / 100);
                    lat = deg + (lat - (100f * deg)) / 60f;

                    if (parts[4] != "N")
                        lat = -lat;

                    var lng = float.Parse(parts[5]);
                    deg = (float)Math.Floor(lng / 100f);
                    lng = deg + (lng - (100f * deg)) / 60f;

                    if (parts[6] != "E")
                        lng = -lng;

                    var speed = float.Parse(parts[7]); // speed in knots

                    onPosition(new PositionRecord { lat = lat, lng = lng, speed = speed });
                }
            }
		}
	}
}

