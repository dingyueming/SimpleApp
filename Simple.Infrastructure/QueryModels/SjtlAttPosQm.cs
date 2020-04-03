using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.QueryModels
{
    /// <summary>
    /// 警员打卡位置查询Model
    /// </summary>
    public class SjtlAttPosQm
    {
        public DateTime[] DateTimes { get; set; }

        public List<double[]> Points { get; set; }

        public string NameOrPoliceCode { get; set; }
    }
}
