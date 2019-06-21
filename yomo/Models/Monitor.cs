using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo.Models
{
    public enum OerationModes
    {
        StandBy,
        Haulted,
        Manual,
        AutoRoute,
        AutoRegion
    }

    /// <summary>
    ///  All the stuff we want to see in order to know everything is running ok
    /// </summary>
    public class Monitor
    {
        public OerationModes OperationMode;
        public string SystemStatus;
        
        public DateTime LastFullMow;
        public string CurrentLocationDesc;

        public TimeSpan TotalTimeInService;

        public float BatteryVoltage;
        public float SolarVoltage;

        public bool IsMowing { get; set; }
        public bool IsMoving { get; set; }
        public bool IsBreadCrumbing { get; set; }

    }
}
