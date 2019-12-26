using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Simple.Common
{
    /// <summary>
    /// 返回结果类
    /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = string.Empty;
        /// <summary>
        /// 结果码 0-成功 -1-失败
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// layer图标号 0-黄色感叹图标 1-绿色成功图标 2-红色错误图标 3-黄色询问图标 4-黑色锁图标 5-红色哭丧脸图标 6-绿色笑脸图标
        /// </summary>
        public int icon { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public object tag { get; set; }
        /// <summary>
        /// 附加数据的标记
        /// </summary>
        public string tagext { get; set; }
        /// <summary>
        /// 要跳转的连接
        /// </summary>
        public string url { get; set; }

        public ReturnResult()
        {
            code = -1;
            icon = 2;           
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool Success()
        {
            return code == 0;
        }

        /// <summary>
        /// 以tag方式创建新对象
        /// </summary>
        /// <returns></returns>
        public static ReturnResult NewWithTag(object tag)
        {
            ReturnResult result = new ReturnResult();
            if (tag == null)
            {
                result.msg = "获取数据为空";
                return result;
            }
            if (tag is bool)
            {
                bool suc = (bool)tag;
                if (!suc)
                {
                    result.msg = "操作失败";
                    return result;
                }
            }

            result.tag = tag;
            result.code = 0;
            return result;
        }

        /// <summary>
        /// 根据错误消息创建对象
        /// </summary>
        /// <param name="msg">错误对象</param>
        /// <returns></returns>
        public static ReturnResult NewWithMsg(string msg)
        {
            ReturnResult result = new ReturnResult();
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 获取空数据返回对象
        /// </summary>
        /// <returns></returns>
        public static ReturnResult NewWithNull()
        {
            return NewWithTag(null);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 获取JObject类型的Tag对象
        /// </summary>
        /// <returns></returns>
        public JObject GetJObject()
        {
            return tag as JObject;
        }
        /// <summary>
        /// 获取JArray类型的Tag对象
        /// </summary>
        /// <returns></returns>
        public JArray GetJArray()
        {
            return tag as JArray;
        }

        /// <summary>
        /// 将Tag中的内容转换为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public T TagToObject<T>()
        {
            if (typeof(T).IsArray)
            {
                JArray ja = GetJArray();
                return ja.ToObject<T>();
            }
            else
            {
                JObject jo = GetJObject();
                if (jo == null)
                {
                    JArray ja = GetJArray();
                    if (ja != null)
                        return ja.ToObject<T>();
                    else
                        return default(T);
                }
                else
                {
                    return jo.ToObject<T>();
                }
            }
        }
    }
}
