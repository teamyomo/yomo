using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yomo.Models
{
    public enum Shape
    {
        Region,
        Route        
    }

    public class Geometry
    {
        public string Id;
        public string Name { get; set; }
        public Shape Shape {get; set; }

        public double[] Coordinates; // every other lat/long
    }

    public class GeometryCatelogEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Shape Shape { get; set; }
    }


    public class Configuration
    {
        public float Speed;
        public float TurnSpeed;
        public float DeckWidth;
        public float Overlap;
        public float OffsetX;
        public float OffsetY;
        public decimal BaseLat;
        public decimal BaseLong;
    }
}
