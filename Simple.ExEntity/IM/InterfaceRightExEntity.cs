using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.IM
{
    public class InterfaceRightExEntity
    {
        #region Model
        /// <summary>
        /// 接口应用id
        /// </summary>
        public virtual int App_Id { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public virtual string Org_Code { get; set; }

        public virtual UnitExEntity Unit { get; set; }
        #endregion
    }
}
