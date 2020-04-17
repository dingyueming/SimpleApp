using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("tbl_interface")]
    public class InterfaceEntity
    {
        #region Model
        /// <summary>
        /// 应用名称
        /// </summary>
        public virtual string App_Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public virtual string Ip { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public virtual int Port { get; set; }
        /// <summary>
        /// 接口类型 1 接入 2 转发
        /// </summary>
        public virtual int Int_Type { get; set; }
        /// <summary>
        /// 终端类型 1 车辆 2 对讲机
        /// </summary>
        public virtual int Ter_Type { get; set; }
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public virtual int App_Id { get; set; }
        /// <summary>
        /// 1 启用 2 停用
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 运行状态 1 正常 2 异常
        /// </summary>
        public virtual int Run_Status { get; set; }
        /// <summary>
        /// 接口标准 1 省市平台接口标准 2 其他警种转发标准 3 定制标准
        /// </summary>
        public virtual int Proto_Type { get; set; }

        /// <summary>
        /// 组织机构编码集合
        /// </summary>
        [Computed]
        public List<InterfaceRightEntity> RightEntities { get; set; } = new List<InterfaceRightEntity>();
        #endregion
    }
}
