using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    /// <summary>
    /// 短报文实体
    /// </summary>
    public class UpMsg
    {
        /// <summary>
        /// mac
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public UpMsgContent Content { get; set; }
    }

    public class UpMsgContent
    {
        /// <summary>
        /// 报文类型 默认 0
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 报文内容，编码GBK
        /// </summary>
        public string Msg { get; set; }
    }
}
