using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    public class BaseCommand<T> where T : class
    {
        public string Mac { get; set; }
        public CommandHead Head { get; set; }
        public T Content { get; set; }
    }
    public class CommandHead
    {
        /// <summary>
        /// 命令ID 
        /// </summary>
        public int COMMAND_ID { get; set; }
        /// <summary>
        /// 命令序号，最大65535，超过65535回绕到1
        /// </summary>
        public int CMD_SEQ { get; set; }
        /// <summary>
        /// 终端类型，数据库cars表mtype字段
        /// </summary>
        public int MOBILE_TYPE { get; set; }
        /// <summary>
        /// 终端接入服务编号，数据库cars表mtype字段
        /// </summary>
        public int CI_SERVERNO { get; set; }
        /// <summary>
        /// 发出命令的用户号
        /// </summary>
        public int USERID { get; set; }
        /// <summary>
        /// 通信代理号，默认0
        /// </summary>
        public int AGENT_NO { get; set; }

    }
}
