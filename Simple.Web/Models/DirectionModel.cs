using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    public class DirectionModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }
        /// <summary>
        /// 目的地名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 距离(M)
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// 预计行驶时间（S）
        /// </summary>
        public int Duration { get; set; }
    }
}
