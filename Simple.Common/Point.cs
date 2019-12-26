using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common
{
   public class Point
    {
        public double X  { get; set; }
        public double Y { get; set; }

        public Point()
        {
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
