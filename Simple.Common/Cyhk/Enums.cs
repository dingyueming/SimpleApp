using Simple.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLiby.Cyhk
{
    /// <summary>
    /// 车辆状态
    /// </summary>
    public enum CarState
    {
        /// <summary>
        /// 无状态
        /// </summary>
        [Description("未知")]
        NoStatus,
        /// <summary>
        /// 不在线
        /// </summary>
        [Description("不在线")]
        OffLine,
        /// <summary>
        /// 不定位
        /// </summary>
        [Description("不定位")]
        NoGps,
        /// <summary>
        /// 停车
        /// </summary>
        [Description("停车中")]
        Stop,
        /// <summary>
        /// 行驶中
        /// </summary>
        [Description("行驶中")]
        Run,
        /// <summary>
        /// 报警
        /// </summary>
        [Description("报警中")]
        Alarm,
    }

    /// <summary>
    /// 树形节点类型
    /// </summary>
    public enum TreeViewItemType
    {
        /// <summary>
        /// 单位
        /// </summary>
        Unit,
        /// <summary>
        /// 车辆
        /// </summary>
        Car
    }
}
