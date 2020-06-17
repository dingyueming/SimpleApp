using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("CAR_MSGREPORT")]
    public class CarMsgReportEntity
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public virtual int ID { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>
        public virtual int CARID { get; set; }
        /// <summary>
        /// 任务内容
        /// </summary>
        public virtual string CONTENT { get; set; }
        /// <summary>
        /// 出动时间
        /// </summary>
        public virtual DateTime? SENDTIME { get; set; }
        /// <summary>
        /// 返回时间
        /// </summary>
        public virtual DateTime? BACKTIME { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public virtual string APPROVER { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int? CREATOR { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CREATETIME { get; set; }
        /// <summary>
        /// 车辆
        /// </summary>
        [Computed]
        public CarEntity Car { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        [Computed]
        public UsersEntity CreateUser { get; set; }
        #endregion
    }
}
