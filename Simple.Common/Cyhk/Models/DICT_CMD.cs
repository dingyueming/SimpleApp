using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLiby.Cyhk.Models
{
    /// <summary>
    /// 命令对象
    /// </summary>
    public class DICT_CMD
    {
        public ushort CMDID { get; set; }
        public string CMDNAME { get; set; }
        public string CMDEN { get; set; }
        public string CMDNAME_RUS { get; set; }
    }
}
