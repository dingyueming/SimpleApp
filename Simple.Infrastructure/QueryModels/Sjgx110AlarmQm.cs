﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.QueryModels
{
    /// <summary>
    /// lbs位置查询
    /// </summary>
    public class Sjgx110AlarmQm
    {
        public DateTime[] DateTimes { get; set; }

        public List<double[]> Points { get; set; }
    }
}
