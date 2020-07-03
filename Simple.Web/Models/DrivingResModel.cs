using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    public class DrivingResModel
    {
        public DrivingRoute Route { get; set; }

        public int status { get; set; }

        public string info { get; set; }

        public string infocode { get; set; }

        public int count { get; set; }
    }

    public class DrivingRoute
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public Path[] Paths { get; set; }
    }

    public class Path
    {
        public Step[] Steps { get; set; }
    }

    public class Step
    {
        public string Polyline { get; set; }
    }
}
