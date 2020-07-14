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

        public string CmdStr
        {
            get
            {
                string msg;
                switch (Cmd)
                {
                    case 5:
                        msg = "位置查询";
                        break;
                    case 6:
                        msg = "设置回传间隔";
                        break;
                    case 4:
                        msg = "发送消息";
                        break;
                    case 36:
                        msg = "下发导航点";
                        break;
                    default:
                        msg = "";
                        break;
                }
                return msg;
            }
        }
        /// <summary>
        /// 界面展示的消息
        /// </summary>
        /// <returns></returns>
        public string ShowMsg
        {
            get
            {
                string msg;
                switch (Status)
                {//0 3 14 22 29 30 31 37 200 201 202 203
                    case 0:
                        msg = "成功";
                        break;
                    case 3:
                        msg = "数据库查询失败";
                        break;
                    case 14:
                        msg = "无效的命令";
                        break;
                    case 22:
                        msg = "找不到对应的终端协议解释程序";
                        break;
                    case 29:
                        msg = "处理异常";
                        break;
                    case 30:
                        msg = "不支持的指令";
                        break;
                    case 31:
                        msg = "终端不在线";
                        break;
                    case 37:
                        msg = "没有该终端";
                        break;
                    case 200:
                        msg = "失败(终端)";
                        break;
                    case 201:
                        msg = "消息有误(终端)";
                        break;
                    case 202:
                        msg = "不支持(终端)";
                        break;
                    case 203:
                        msg = "超时(终端返回)";
                        break;
                    default:
                        msg = "未知命令";
                        break;
                }
                return msg;
            }
        }
    }
}
