using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.QueryModels
{
    public class Sjgx110AlarmQm
    {
        public DateTime[] DateTimes { get; set; }

        public List<double[]> Points { get; set; }
    }
}
