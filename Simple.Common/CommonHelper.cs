using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Common
{
    public static class CommonHelper
    {

        /// <summary>
        /// 获取一个GUID作为数据库表或者表单的主键
        /// </summary>
        /// <returns></returns>
        public static string GetGuidString()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        #region 验证是否为数字格式
        /// <summary>
        /// 验证是否为数字格式（包括浮点型）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        /// <summary>
        /// 验证s是否为数字格式（只限整型）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumricForInt(string str)
        {
            if (str == null || str.Length == 0)
            {
                return false;
            }
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 转换对象类型
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultValue">转换失败默认值</param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, T defaultValue = default(T))
        {
            if (obj == null) return defaultValue;

            try
            {
                T result = (T)Convert.ChangeType(obj, typeof(T));
                if (result.Equals(default(T)))
                    result = defaultValue;
                return result;
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="val">数据值</param>
        /// <returns></returns>
        public static object Convert2Type(Type type, object val)
        {
            try
            {
                return Convert.ChangeType(val, type);
            }
            catch (Exception ex)
            {
                return "err_" + ex.Message;
            }
        }

    }
}
