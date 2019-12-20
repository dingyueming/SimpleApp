using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.CommonModel.EzMap
{
    /// <summary>
    /// 地图类
    /// </summary>
    public class EzMarker
    {
        /// <summary>
        /// 地理坐标
        /// </summary>
        public EzCoord Coord { get; set; }
        /// <summary>
        /// Icon对象,默认由系统提供
        /// </summary>
        public EzIcon Icon { get; set; }
        /// <summary>
        /// Title对象,标题
        /// </summary>
        public EzTitle Title { get; set; }
        /// <summary>
        /// 其他属性
        /// </summary>
        public object Options { get; set; }
    }
    /// <summary>
    /// 地理坐标
    /// </summary>
    public class EzCoord
    {
        /// <summary>
        /// 大地经度
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 大地纬度
        /// </summary>
        public int Y { get; set; }
    }
    /// <summary>
    /// Icon对象,默认由系统提供
    /// </summary>
    public class EzIcon
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Src { get; set; }
        /// <summary>
        /// 图片大小
        /// </summary>
        /// <example>[25,4]</example>
        public int[] Size { get; set; }
        /// <summary>
        /// 默认图片左上角点
        /// </summary>
        /// <example>[0,0]</example>
        public string Anchor { get; set; }
        /// <summary>
        /// anchorX值单位:'fraction'(比例,0~1,默认值),'pixels'(像素值,不超过图像X轴长)
        /// </summary>
        public string AnchorXUits { get; set; }
        /// <summary>
        /// anchorY值单位:'fraction'(比例,0~1,默认值),'pixels'(像素值,不超过图像X轴长)
        /// </summary>
        public string AnchorYUits { get; set; }
        /// <summary>
        /// 图像缩放系数
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 图像透明度
        /// </summary>
        public string Opacity { get; set; }
    }
    /// <summary>
    /// Title对象,标题
    /// </summary>
    public class EzTitle
    {

    }
}
