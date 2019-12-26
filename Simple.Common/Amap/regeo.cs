using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Json;
using Newtonsoft.Json;

namespace Common.Amap
{
    /// <summary>
    /// 高德地图你地理编码API服务
    /// 地址：http://restapi.amap.com/v3/geocode/regeo?parameters
    /// 文档：http://lbs.amap.com/api/webservice/guide/api/georegeo/
    /// </summary>
    public class regeo
    {
        /// <summary>
        /// 返回结果状态值
        /// 返回值为 0 或 1，0 表示请求失败；1 表示请求成功。
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 返回状态说明
        /// 当 status 为 0 时，info 会返回具体错误原因，否则返回“OK”。
        /// 详情可以参考 http://lbs.amap.com/api/webservice/guide/tools/info
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 逆地理编码
        /// batch 为false 时为单个请求，会返回 regeocode 对象
        /// </summary>
        public Regeocode regeocode { get; set; }
        /// <summary>
        /// 逆地理编码列表
        /// batch 字段设置为 true 时为批量请求，此时 regeocodes 标签返回，标签下为 regeocode 对象列表；
        /// </summary>
        public Regeocode[] regeocodes { get; set; }

    }

    /// <summary>
    /// 逆地理编码对象
    /// </summary>
    public class Regeocode
    {
        /// <summary>
        /// 结构化地址信息
        /// 省份＋城市＋区县＋城镇＋乡村＋街道＋门牌号码
        /// 如果坐标点处于海域范围内，则结构化地址信息为：省份＋城市＋区县＋海域信息
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string formatted_address { get; set; }
        /// <summary>
        /// 地址元素列表
        /// </summary>
        public AddressComponent addressComponent { get; set; }
        /// <summary>
        /// 道路信息列表
        /// </summary>
        public road[] roads { get; set; }
        /// <summary>
        /// 道路交叉口列表
        /// </summary>
        public roadinter[] roadinters { get; set; }
        /// <summary>
        /// poi信息列表
        /// </summary>
        public poi[] pois { get; set; }
        /// <summary>
        /// aoi信息列表
        /// </summary>
        public aoi[] aois { get; set; }
    }
    /// <summary>
    /// 地址元素列表
    /// </summary>
    public class AddressComponent
    {
        /// <summary>
        /// 坐标点所在省名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string province { get; set; }
        /// <summary>
        /// 坐标点所在城市名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string city { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string citycode { get; set; }
        /// <summary>
        /// 坐标点所在区
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string district { get; set; }
        /// <summary>
        /// 行政区编码
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string adcode { get; set; }
        /// <summary>
        /// 坐标点所在乡镇/街道（此街道为社区街道，不是道路信息）
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string township { get; set; }
        /// <summary>
        /// 乡镇街道编码
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string towncode { get; set; }
        /// <summary>
        /// 社区信息列表
        /// </summary>
        public Neighborhood neighborhood { get; set; }
        /// <summary>
        /// 楼信息列表
        /// </summary>
        public Building building { get; set; }
        /// <summary>
        /// 门牌信息列表
        /// </summary>
        public StreetNumber streetNumber { get; set; }
        /// <summary>
        /// 所属海域信息
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string seaArea { get; set; }
        /// <summary>
        /// 经纬度所属商圈列表
        /// </summary>
        //public BusinessAreas[] businessAreas { get; set; }
    }

    /// <summary>
    /// 社区信息
    /// </summary>
    public class Neighborhood
    {
        /// <summary>
        /// 社区名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// POI类型
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string type { get; set; }
    }
    /// <summary>
    /// 楼信息
    /// </summary>
    public class Building
    {
        /// <summary>
        /// 建筑名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string type { get; set; }
    }
    /// <summary>
    /// 门牌信息
    /// </summary>
    public class StreetNumber
    {
        /// <summary>
        /// 街道名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string street { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string number { get; set; }
        /// <summary>
        /// 坐标点
        /// 经纬度坐标点：经度，纬度
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string direction { get; set; }
        /// <summary>
        /// 门牌地址到请求坐标的距离 
        /// 单位：米
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string distance { get; set; }
    }
    /// <summary>
    /// 经纬度所属商圈
    /// </summary>
    public class BusinessAreas
    {
        /// <summary>
        /// 商圈信息
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string businessArea { get; set; }
        /// <summary>
        /// 商圈中心点经纬度
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
        /// <summary>
        /// 商圈名称 
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// 商圈所在区域的adcode 
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string id { get; set; }
    }

    public class road
    {
        /// <summary>
        /// 道路id
        /// </summary>
        [JsonConverter(typeof(StringConverter))]

        public string id { get; set; }
        /// <summary>
        /// 道路名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// 方位
        /// </summary>
        [JsonConverter(typeof(StringConverter))]

        public string direction { get; set; }
        /// <summary>
        /// 道路到请求坐标的距离
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string distance { get; set; }
        /// <summary>
        /// 坐标点
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
    }
    /// <summary>
    /// 道路交叉口
    /// </summary>
    public class roadinter
    {
        /// <summary>
        /// 交叉路口到请求坐标的距离
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string direction { get; set; }
        /// <summary>
        /// 方位
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string distance { get; set; }
        /// <summary>
        /// 路口经纬度
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
        /// <summary>
        /// 第一条道路id
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string first_id { get; set; }
        /// <summary>
        /// 第一条道路名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string first_name { get; set; }
        /// <summary>
        /// 第二条道路id
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string second_id { get; set; }
        /// <summary>
        /// 第二条道路名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string second_name { get; set; }
    }

    /// <summary>
    /// poi信息
    /// </summary>
    public class poi
    {
        /// <summary>
        /// poi的id        
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string id { get; set; }
        /// <summary>
        /// poi点名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// poi类型
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string type { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string tel { get; set; }
        /// <summary>
        /// 该POI到请求坐标的距离 单位：米        
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string distance { get; set; }
        /// <summary>
        /// 方向        
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string direction { get; set; }
        /// <summary>
        /// poi地址信息
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string address { get; set; }
        /// <summary>
        /// 坐标点
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
        /// <summary>
        /// poi所在商圈名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string businessarea { get; set; }
    }

    /// <summary>
    /// aoi信息
    /// </summary>
    public class aoi
    {
        /// <summary>
        /// 所属 aoi的id
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string id { get; set; }
        /// <summary>
        /// 所属 aoi 名称
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string name { get; set; }
        /// <summary>
        /// 所属 aoi 所在区域编码
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string adcode { get; set; }
        /// <summary>
        /// 所属 aoi 中心点坐标
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string location { get; set; }
        /// <summary>
        /// 所属aoi点面积 单位：平方米
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string area { get; set; }
        /// <summary>
        /// 输入经纬度是否在aoi面之中
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string distance { get; set; }
        /// <summary>
        /// aoi类型
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        public string type { get; set; }
    }
}
