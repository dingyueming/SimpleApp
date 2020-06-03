using Simple.Web.Models.CmdInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    /// <summary>
    /// 用户发出的指令
    /// </summary>
    public class CmdByUser : CommandHead
    {
        /// <summary>
        /// 用户链接id
        /// </summary>
        public string ConnId { get; set; }

        #region 重写equals和gethashcode
        public override bool Equals(object obj)
        {
            if (!(obj is BaseCommand<CmdResponse>))
            {
                return false;
            }
            var response = obj as BaseCommand<CmdResponse>;
            if (response.Head.CMD_SEQ == CMD_SEQ && response.Content.Cmd == COMMAND_ID && response.Head.USERID == USERID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
