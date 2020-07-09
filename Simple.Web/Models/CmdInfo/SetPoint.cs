using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models.CmdInfo
{
    /// <summary>
    /// 下发导航点
    /// </summary>
    public class SetPoint
    {
        /// <summary>
        /// 0 东纳 1 美行 消防为0
        /// </summary>
        public int Map_Type { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 路径选择
        /// </summary>
        /// <remarks>
        /// 0:任意；1:导航地图自定(推荐）; 2:最短路径; 3:最少收费(经济）; 4:高速优先（最快）
        /// </remarks>
        public int Path { get; set; }
        /// <summary>
        /// 目的地名称 最长86字节
        /// </summary>
        public string Name { get; set; }
    }
}
