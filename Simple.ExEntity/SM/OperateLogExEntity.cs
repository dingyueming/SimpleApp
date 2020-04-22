using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.SM
{
    public class OperateLogExEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
        public virtual int Logid { get; set; }
        /// <summary>
        /// 登陆名称
        /// </summary>
        public virtual string Loginname { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string Realname { get; set; }
        /// <summary>
        /// 登陆IP
        /// </summary>
        public virtual string Ip { get; set; }
        /// <summary>
        /// -1退出,0登陆,1增,2删,3改
        /// </summary>
        public virtual int Operatetype { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual string OperatetypeShow
        {
            get
            {
                string str;
                switch (Operatetype)
                {
                    case -1:
                        str = "退出";
                        break;
                    case 0:
                        str = "登陆";
                        break;
                    case 1:
                        str = "增加";
                        break;
                    case 2:
                        str = "删除";
                        break;
                    case 3:
                        str = "修改";
                        break;
                    default:
                        str = "";
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Operatetime => DateTime.Now;
        /// <summary>
        /// 日志对象名
        /// </summary>
        public virtual string Modelname { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        #endregion
    }
}
