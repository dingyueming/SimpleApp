using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models.CmdInfo
{
    /// <summary>
    /// 命令响应
    /// </summary>
    public class CmdResponse
    {
        /// <summary>
        /// 被确认的命令
        /// </summary>
        public int Cmd { get; set; }
        /// <summary>
        /// 状态，0 成功 其他失败，失败值参考数据库 dict_status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 界面展示的消息
        /// </summary>
        /// <returns></returns>
        public string ShowMsg
        {
            get
            {
                string msg;
                switch (Cmd)
                {
                    case 5:
                        msg = "位置查询";
                        break;
                    default:
                        msg = "命令反馈";
                        break;
                }
                msg += Status == 0 ? "成功" : "失败";
                return msg;
            }
        }
    }
}
