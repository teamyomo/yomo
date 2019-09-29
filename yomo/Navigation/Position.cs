
using crozone.LinuxSerialPort;
using System;
using System.IO;
using System.IO.Ports;
using yomo.Utility;

namespace yomo.Navigation
{
    public enum GPSStatus
    {
        DataNotValid = 'N',
        AutonomousMode = 'A',
        DifferentialMode = 'D',
        EstimatedMode = 'E'
    }

    public class Position : IPosition
    {
        const string portName = "/dev/serial0";

        public class PositionRecord
        {
            public Vector position = new Vector(0,0);
            public float speed;
            public float course;
            public GPSStatus mode;
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

                    onPosition(new PositionRecord()
                    {
                        position = new Vector(ParseCoordinate(parts[5], parts[6][0]), ParseCoordinate(parts[3], parts[4][0])),
                        speed = float.Parse(parts[7]),
                        course = float.Parse(parts[8]),
                        mode = (GPSStatus)Enum.Parse(typeof(GPSStatus), parts[10])
                    });
                }
            }
        }

        /// <summary>
        /// Convert
        ///  dddmm.mmmm into double precision degrees
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        private static double ParseCoordinate(string value, char cardinal)
        {
            // Use the period to split the coordinate... Don't assume zero padding on whole degrees
            var decMk = value.IndexOf('.');
            // Two digits left of decimal, split whole degrees and fractional minutes
            var degrees = short.Parse(value.Substring(0, decMk - 2));
            var minutes = double.Parse(value.Substring(decMk - 2));
            var coord = (double)degrees + minutes / 60d; // put it all together and convert minutes to degrees

            return (cardinal != 'N' && cardinal != 'E') ? coord : -coord;
        }
    }
}

