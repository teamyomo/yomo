using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo
{

    public enum Mower{on, off};
    public enum BreadCrumb { on, off};

    public class Command
    {
        public bool AllStop { get; set; } // All stop! everything off

        public int LeftSlider { get; set; }
        public int RightSlider { get; set; }

        public Mower Mower { get; set; }

        /// <summary>
        /// Used to record GPS tracks for regions or routes
        /// </summary>
        public BreadCrumb BreadCrumb { get; set; } 

        public string StartAutoMowRegion { get; set; } // Region to start automow, empty for manual mode
    }
}
