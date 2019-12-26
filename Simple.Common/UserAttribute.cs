using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simple.Common
{
    /// <summary>
    /// 自定义特性类(描述)
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        // Fields
        public string Text;

        public int Tag;

        // Methods
        public DescriptionAttribute(string text, int tag = 0)
        {
            this.Text = text;
            this.Tag = tag;
        }

        /// <summary>
        /// 获取描述特性的值
        /// </summary>
        /// <param name="obj">拥有描述特性的对象</param>
        /// <returns></returns>
        public static string GetText(object obj)
        {
            MemberInfo[] memInfo = obj.GetType().GetMember(obj.ToString());
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Text;
                }
            }
            return obj.ToString();
        }

        /// <summary>
        /// 获取描述特性的值
        /// </summary>
        /// <param name="obj">拥有描述特性的对象</param>
        /// <returns></returns>
        public static DescriptionAttribute GetDescription(object obj)
        {
            MemberInfo[] memInfo = obj.GetType().GetMember(obj.ToString());
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]);
                }
            }
            return null;
        }

        public static string GetNameFromText<T>(string text)
        {
            MemberInfo[] memInfo = typeof(T).GetMembers();
            if (memInfo.Length > 0)
            {
                foreach (MemberInfo minfo in memInfo)
                {
                    object[] attrs = minfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        if (((DescriptionAttribute)attrs[0]).Text == text)
                            return minfo.Name;
                    }
                }
            }
            return null;
        }

        public static string[] GetTextsFromType<T>()
        {
            MemberInfo[] memInfo = typeof(T).GetMembers();
            List<string> list = new List<string>();
            if (memInfo.Length > 0)
            {
                for (int i = 0; i < memInfo.Length; i++)
                {
                    MemberInfo minfo = memInfo[i];
                    object[] attrs = minfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        list.Add(((DescriptionAttribute)attrs[0]).Text);
                    }
                }
            }
            return list.ToArray();
        }


        public override string ToString()
        {
            return Text;
        }
    }
}
