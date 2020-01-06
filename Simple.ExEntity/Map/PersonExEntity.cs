using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.Map
{
    public class PersonExEntity
    {
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINAL_CODE { get; set; }
        /// <summary>
        /// 警号
        /// </summary>
        public string POLICE_CODE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
    }
}
