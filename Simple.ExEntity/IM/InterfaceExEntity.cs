using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.ExEntity.IM
{
    public class InterfaceExEntity
    {
        public InterfaceExEntity()
        {
            Right = new List<InterfaceRightExEntity>();
        }

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
        /// 接口类型 1 接入 2 转发
        /// </summary>
        public virtual string Int_TypeShow
        {
            get
            {
                return Int_Type == 1 ? "接入" : "转发";
            }
        }
        /// <summary>
        /// 终端类型 1 车辆 2 对讲机
        /// </summary>
        public virtual int Ter_Type { get; set; }
        /// <summary>
        /// 终端类型 1 车辆 2 对讲机
        /// </summary>
        public virtual string Ter_TypeShow
        {
            get
            {
                return Ter_Type == 1 ? "车辆" : "对讲机";
            }
        }
        /// <summary>
        /// id
        /// </summary>
        public virtual int App_Id { get; set; }
        /// <summary>
        /// 1 启用 2 停用
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 1 启用 2 停用
        /// </summary>
        public virtual string StatusShow
        {
            get
            {
                return Status == 1 ? "启用" : "停用";
            }
        }
        /// <summary>
        /// 运行状态 1 正常 2 异常
        /// </summary>
        public virtual int Run_Status { get; set; }
        /// <summary>
        /// 运行状态 1 正常 2 异常
        /// </summary>
        public virtual string Run_StatusShow
        {
            get
            {
                return Run_Status == 1 ? "正常" : "异常";
            }
        }
        /// <summary>
        /// 接口标准 1 省市平台接口标准 2 其他警种转发标准 3 定制标准
        /// </summary>
        public virtual int Proto_Type { get; set; }
        /// <summary>
        /// 接口标准 1 省市平台接口标准 2 其他警种转发标准 3 定制标准
        /// </summary>
        public virtual string Proto_TypeShow
        {
            get
            {
                switch (Proto_Type)
                {
                    case 1:
                        return "省市平台接口标准";
                    case 2:
                        return "其他警种转发标准";
                    default:
                        return "定制标准";
                }
            }
        }
        /// <summary>
        /// 组织机构编码集合
        /// </summary>
        public List<InterfaceRightExEntity> Right { get; set; }

        public string[] Orgcode
        {
            get
            {
                return Right.Select(x => x.Org_Code).ToArray();
            }
        }
        #endregion
    }
}
