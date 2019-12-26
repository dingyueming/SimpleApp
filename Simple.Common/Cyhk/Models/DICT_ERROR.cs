using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLiby.Cyhk.Models
{
    /// <summary>
    /// 命令返回错误状态
    /// </summary>
    public class DICT_ERROR
    {
        /// <summary>
        /// ID号
        /// </summary>
        public ushort NO { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string EDESC { get; set; }
        /// <summary>
        /// 错误描述英语
        /// </summary>
        public string DESC_EN { get; set; }
        /// <summary>
        /// 错误描述中文
        /// </summary>
        public string DESC_CN { get; set; }
        /// <summary>
        /// 错误描述俄语
        /// </summary>
        public string EDESC_RUS { get; set; }
    }
}
