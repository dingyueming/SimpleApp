using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("sjtl_attendance_position")]
    public class SjtlAttendancePositionEntity
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public virtual int Id { get; set; }
        /// <summary>
        /// 警号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public virtual double Jd { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual double Wd { get; set; }
        /// <summary>
        /// 部门代码
        /// </summary>
        public virtual string Deptcode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public virtual string Deptname { get; set; }
        /// <summary>
        /// 考勤时间
        /// </summary>
        public virtual DateTime Check_Time { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public virtual DateTime Insert_Date { get; set; }

        #endregion
    }
}
