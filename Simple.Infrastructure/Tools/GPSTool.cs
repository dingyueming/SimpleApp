using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Infrastructure.Tools
{
    public class GPSTool
    {
        /// <summary>
        /// 根据航向获得航向字符串
        /// </summary>
        /// <param name="heading">航向（单位度）</param>
        /// <returns></returns>
        public static string GetHeadingStr(float heading)
        {
            if (heading > 337 && heading < 23)
                return "北";
            if (heading > 22 && heading < 68)
                return "东北";
            if (heading > 67 && heading < 113)
                return "东";
            if (heading > 112 && heading < 158)
                return "东南";
            if (heading > 157 && heading < 203)
                return "南";
            if (heading > 202 && heading < 248)
                return "西南";
            if (heading > 247 && heading < 293)
                return "西";
            if (heading > 292 && heading < 338)
                return "西北";
            return "北";
        }

        /// <summary>
        /// 获得反方向字符串
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static string GetOppositeDirection(string direction)
        {
            string res = "未知";
            switch (direction)
            {
                case "北":
                    res = "南";
                    break;
                case "东北":
                    res = "西南";
                    break;
                case "东":
                    res = "西";
                    break;
                case "东南":
                    res = "西北";
                    break;
                case "南":
                    res = "北";
                    break;
                case "西南":
                    res = "东北";
                    break;
                case "西":
                    res = "东";
                    break;
                case "西北":
                    res = "东南";
                    break;
                default:
                    break;
            }
            return res;
        }

        /// <summary>
        /// 根据定位标记得到描述
        /// </summary>
        /// <param name="locate"></param>
        /// <returns></returns>
        public static string GetLocateStr(byte locate)
        {
            if (locate == 0) return "未定位";
            else if (locate == 1) return "已定位";
            else
                return "";
        }

        /// <summary>
        /// 根据定位模式标记得到描述
        /// </summary>
        /// <param name="locate"></param>
        /// <returns></returns>
        public static string GetLocateModeStr(byte locate)
        {
            List<string> modeList = new List<string>();

            if ((locate & 0x1) == 0x1) { modeList.Add("GPS"); }
            if ((locate & 0x2) == 0x2) { modeList.Add("北斗"); }     
            if ((locate & 0x8) == 0x8) {modeList.Add("北斗一代");}

            return string.Join("+", modeList);
        }

        /// <summary>
        /// 根据定位模式标记得到辅助定位描述
        /// </summary>
        /// <returns></returns>
        public static string GetAssistLocateModeStr(byte locate)
        {
            List<string> modeList = new List<string>();
            if ((locate & 0x4) == 0x4)
            {
                modeList.Add("惯导");
            }
            return string.Join("+", modeList);
        }

        private const double x_PI = 3.14159265358979324 * 3000.0 / 180.0;
        private const double PI = 3.1415926535897932384626;
        private const double ee = 0.00669342162296594323;
        private const double a = 6378245.0;
        private const double r = 6378187.0;
        /// <summary>
        ///  判断是否在国内
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static bool out_of_china(double lng, double lat)
        {
            return !(lng > 73.66 && lng < 135.05 && lat > 3.86 && lat < 53.55);
        }
        /// <summary>
        /// 判断是否在国内
        /// </summary>
        /// <param name="location">坐标点</param>
        /// <returns></returns>
        public static bool out_of_china(GeoLatLng location)
        {
            if (location == null) return false;
            return out_of_china(location.longitude, location.latitude);
        }

        private static double transformlng(double lng, double lat)
        {
            double ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng + 0.1 * lng
                    * lat + 0.1 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng
                    * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lng * PI) + 40.0 * Math.Sin(lng / 3.0 * PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lng / 12.0 * PI) + 300.0 * Math.Sin(lng / 30.0
                    * PI)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformlat(double lng, double lat)
        {
            double ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat + 0.1
                    * lng * lat + 0.2 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng
                    * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * PI) + 40.0 * Math.Sin(lat / 3.0 * PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lat / 12.0 * PI) + 320 * Math.Sin(lat * PI
                    / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// GCJ02 转换为 WGS84
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static GeoLatLng gcj02towgs84(double lng, double lat)
        {
            GeoLatLng wgs84Pt = new GeoLatLng(lat, lng);

            if (out_of_china(lng, lat))
            {
                return wgs84Pt;
            }
            else
            {
                double dlat = transformlat(lng - 105.0, lat - 35.0);
                double dlng = transformlng(lng - 105.0, lat - 35.0);
                double radlat = lat / 180.0 * PI;
                double magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                double sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                double mglat = lat + dlat;
                double mglng = lng + dlng;
                wgs84Pt.longitude = lng * 2 - mglng;
                wgs84Pt.latitude = lat * 2 - mglat;
                return wgs84Pt;
            }
        }

        /// <summary>
        /// WGS84转GCj02
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static GeoLatLng wgs84togcj02(double lng, double lat)
        {
            GeoLatLng gcjPt = new GeoLatLng(lat, lng);
            if (out_of_china(lng, lat))
            {
                return gcjPt;
            }
            else
            {
                double dlat = transformlat(lng - 105.0, lat - 35.0);
                double dlng = transformlng(lng - 105.0, lat - 35.0);
                double radlat = lat / 180.0 * PI;
                double magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                double sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                gcjPt.longitude = lng + dlng;
                gcjPt.latitude = lat + dlat;
                return gcjPt;
            }
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static GeoLatLng gcj02tobd09(double lng, double lat)
        {
            double z = Math.Sqrt(lng * lng + lat * lat) + 0.00002
                    * Math.Sin(lat * x_PI);
            double theta = Math.Atan2(lat, lng) + 0.000003 * Math.Cos(lng * x_PI);
            var bd_lng = z * Math.Cos(theta) + 0.0065;
            var bd_lat = z * Math.Sin(theta) + 0.006;
            return new GeoLatLng(bd_lat, bd_lng);
        }
        /// <summary>
        /// 百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换 即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="bd_lng">经度</param>
        /// <param name="bd_lat">纬度</param>
        /// <returns></returns>
        public static GeoLatLng bd09togcj02(double bd_lng, double bd_lat)
        {
            double x = bd_lng - 0.0065;
            double y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_PI);
            var gg_lng = z * Math.Cos(theta);
            var gg_lat = z * Math.Sin(theta);
            return new GeoLatLng(gg_lat, gg_lng);
        }

        /// <summary>
        /// 谷歌经纬度转Tile值
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns></returns>
        public static TileXY GoogleLatLng2TileXY(double lng, double lat, int zoom)
        {
            var lat_rad = lat * PI / 180;
            var n = Math.Pow(2.0, zoom);
            var xtile = (lng + 180.0) / 360.0 * n;
            var ytile = (1.0 - Math.Log(Math.Tan(lat_rad) + (1 / Math.Cos(lat_rad))) / PI) / 2.0 * n;
            return new TileXY((int)xtile, (int)ytile, zoom);
        }
        /// <summary>
        /// 谷歌Tile值转经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static GeoLatLng GoogleTileXY2latlng(int x, int y, int zoom) //google行列号转经纬度
        {
            double n = Math.Pow(2.0, zoom);
            double lon = x / n * 360.0 - 180.0;
            double lat_rad = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * y / n)));
            double lat = lat_rad * 180 / Math.PI;
            return new GeoLatLng(lat, lon);
        }

        //以下是根据百度地图JavaScript API破解得到 百度坐标<->墨卡托坐标 转换算法
        private static double[] array1 = { 75, 60, 45, 30, 15, 0 };
        private static double[] array3 = { 12890594.86, 8362377.87, 5591021, 3481989.83, 1678043.12, 0 };
        private static double[][] array2 = {new double[]{-0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -35149669176653700, 26595700718403920, -10725012454188240, 1800819912950474, 82.5}
                                               ,new double[]{0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167, -5124939663.577472, 913311935.9512032, 67.5}
                                               ,new double[]{0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5}
                                               ,new double[]{0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5}
                                               ,new double[]{-0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5}
                                               ,new double[]{-0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45}};
        private static double[][] array4 = {new double[]{1.410526172116255e-8, 0.00000898305509648872, -1.9939833816331, 200.9824383106796, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2}
                                               ,new double[]{-7.435856389565537e-9, 0.000008983055097726239, -0.78625201886289, 96.32687599759846, -1.85204757529826, -59.36935905485877, 47.40033549296737, -16.50741931063887, 2.28786674699375, 10260144.86}
                                               ,new double[]{-3.030883460898826e-8, 0.00000898305509983578, 0.30071316287616, 59.74293618442277, 7.357984074871, -25.38371002664745, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37}
                                               ,new double[]{-1.981981304930552e-8, 0.000008983055099779535, 0.03278182852591, 40.31678527705744, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06}
                                               ,new double[]{3.09191371068437e-9, 0.000008983055096812155, 0.00006995724062, 23.10934304144901, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4}
                                               ,new double[]{2.890871144776878e-9, 0.000008983055095805407, -3.068298e-8, 7.47137025468032, -0.00000353937994, -0.02145144861037, -0.00001234426596, 0.00010322952773, -0.00000323890364, 826088.5}};

        /// <summary>
        /// 百度经纬度转Tile值
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns></returns>
        public static TileXY BDlatlng2TileXY(double lng, double lat, int zoom)//x,y百度坐标 zoom 放大级别
        {
            MercatorPoint p = BDLatLng2Mercator(new GeoLatLng(lat, lng));
            double x1 = (p.x / Math.Pow(2, (18 - zoom))) / 256;
            double y1 = (p.y / Math.Pow(2, (18 - zoom))) / 256;
            x1 = Math.Pow(2, zoom - 26) * (PI * lng * r / 180);
            y1 = Math.Pow(2, zoom - 26) * r * Math.Log(Math.Tan(PI * lat / 180) + 1 / (Math.Cos(PI * lat / 180)));
            return new TileXY((int)x1, (int)y1, zoom);
        }
        private static double[] BDConvertor(double x, double y, double[] param)
        {
            var T = param[0] + param[1] * Math.Abs(x);
            var cC = Math.Abs(y) / param[9];
            var cF = param[2] + param[3] * cC + param[4] * cC * cC + param[5] * cC * cC * cC + param[6] * cC * cC * cC * cC + param[7] * cC * cC * cC * cC * cC + param[8] * cC * cC * cC * cC * cC * cC;
            T *= (x < 0 ? -1 : 1);
            cF *= (y < 0 ? -1 : 1);
            return new double[] { T, cF };
        }
        /// <summary>
        /// 墨卡托转百度经纬度
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static GeoLatLng Mercator2BDLatLng(MercatorPoint p)
        {
            double[] arr = null;
            MercatorPoint np = new MercatorPoint(Math.Abs(p.x), Math.Abs(p.x));
            for (var i = 0; i < array3.Length; i++)
            {
                if (np.y >= array3[i])
                {
                    arr = array4[i];
                    break;
                }
            }
            double[] res = BDConvertor(np.x, np.y, arr);
            return new GeoLatLng(res[1], res[0]);
        }
        /// <summary>
        /// 百度坐标转墨卡托
        /// </summary>
        /// <param name="p">百度坐标点</param>
        /// <returns></returns>
        public static MercatorPoint BDLatLng2Mercator(GeoLatLng p)
        {
            double[] arr = null;
            double n_lat = p.latitude > 74 ? 74 : p.latitude;
            n_lat = n_lat < -74 ? -74 : n_lat;
            for (var i = 0; i < array1.Length; i++)
            {
                if (p.latitude >= array1[i])
                {
                    arr = array2[i];
                    break;
                }
            }
            if (arr == null)
            {
                for (var i = array1.Length - 1; i >= 0; i--)
                {
                    if (p.latitude <= -array1[i])
                    {
                        arr = array2[i];
                        break;
                    }
                }
            }
            double[] res = BDConvertor(p.longitude, p.latitude, arr);
            return new MercatorPoint((float)res[0], (float)res[1]);
        }


        /// <summary>
        /// 检查经纬度是否有效
        /// </summary>
        /// <param name="la">纬度</param>
        /// <param name="lo">经度</param>
        /// <returns></returns>
        public static bool CheckLaLo(double la, double lo)
        {
            if (la == 0 && lo == 0 || la == 1 && lo == 1) return false;
            if (lo < -180 || la < -90 || lo > 180 || la > 90) return false;
            return true;
        }

        static int[] baiduX = { 0, 0, 1, 3, 5, 10, 20, 42, 84, 169, 339, 678, 1357, 2715, 5431, 10862, 21724, 43448 };
        static int[] baiduY = { 0, 0, 0, 1, 2, 3, 8, 16, 32, 65, 131, 262, 525, 1050, 2100, 4200, 8401, 16802 };
        static int[] googleX = { 0, 1, 3, 7, 13, 26, 52, 106, 212, 425, 851, 1702, 3405, 6811, 13623, 27246, 54492, 108984 };
        static int[] googleY = { 0, 0, 1, 2, 5, 12, 23, 47, 95, 190, 380, 761, 1522, 3045, 6091, 12183, 24366, 48733 };

        public static void GoogleToBaiDuTile(ref int x, ref int y, int z)
        {
            int bx = baiduX[z - 1];// 395
            int gx = googleX[z - 1];// 11:843,12:1685
            // int gx = g + (x-b);// --- 1587+
            x = x - gx + bx;// --- 1587+
            // 谷歌瓦片行编号=[谷歌参照瓦片行编号+(百度行编号 – 百度参照瓦片行编号)]

            int by = baiduY[z - 1];// 147
            int gy = googleY[z - 1];// 10:
            // int gy = g - (y-b);//
            y = gy + by - y;//
            // 谷歌瓦片列编号=[谷歌参照瓦片列编号- (百度列编号 – 百度参照瓦片列编号)] //向上，列为递减

        }
    }
    /// <summary>
    /// 坐标点
    /// </summary>
    public class GeoLatLng
    {

        public GeoLatLng(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public override string ToString()
        {
            return "lat:" + latitude.ToString() + " lng:" + longitude.ToString();
        }
        public double latitude;
        public double longitude;
    }
    /// <summary>
    /// 地图XY切片
    /// </summary>
    public class TileXY
    {
        public TileXY(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
    /// <summary>
    /// 墨卡托坐标点
    /// </summary>
    public class MercatorPoint
    {
        public MercatorPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float x { get; set; }
        public float y { get; set; }
    }

    public enum GeoType
    {
        WGS84,
        GCJ02,
        BD09
    }
}
