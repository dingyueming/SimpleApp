using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Common.Compress;

namespace Simple.Common
{
    public class StringPlus
    {
        #region 字符串分割与合并
        //private static readonly object getstrarraylock = new object();
        public static List<string> GetStrArray(string str, string speater = "_;_", bool toLower = false, bool removeEmpty = false)
        {
            //lock (getstrarraylock)
            {
                List<string> list = new List<string>();
                string[] ss = str.Split(new[] { speater }, StringSplitOptions.None);
                foreach (string s in ss)
                {
                    if (removeEmpty && string.IsNullOrWhiteSpace(s))
                        continue;

                    string strVal = s;
                    if (toLower)
                        strVal = s.ToLower();

                    list.Add(strVal);
                }
                return list;
            }
        }
        //private static readonly object getarraystrlock = new object();
        public static string GetArrayStr(ICollection collection, string speater = "_;_", string formatStr = "{0}")
        {
            if (collection == null || collection.Count == 0) return null;
            //lock (getarraystrlock)
            {
                Array list = new object[collection.Count];
                collection.CopyTo(list, 0);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Length; i++)
                {
                    object strVal = list.GetValue(i);
                    if (i == list.Length - 1)
                    {
                        sb.Append(string.Format(formatStr, strVal));
                    }
                    else
                    {
                        sb.Append(string.Format(formatStr, strVal));
                        sb.Append(speater);
                    }
                }
                return sb.ToString();
            }
        }
        #endregion

        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region 字符串解/压缩

        /// <summary>
        /// 转换为Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64String(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(compressBeforeByte);
        }
        /// <summary>
        /// 从Base64转换为普通字符串
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string FromBase64String(string base64Str)
        {
            if (string.IsNullOrWhiteSpace(base64Str)) return base64Str;
            byte[] bs = Convert.FromBase64String(base64Str);
            return Encoding.UTF8.GetString(bs, 0, bs.Length);
        }

        /// <summary>
        /// 对字符串进行压缩
        /// </summary>
        /// <param name="str">待压缩的字符串</param>
        /// <returns>压缩后的字符串</returns>
        public static string CompressString(string str)
        {
            string compressString = str;
            try
            {
                byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
                byte[] compressAfterByte = Utils.Compress(compressBeforeByte);

                return Convert.ToBase64String(compressAfterByte);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return compressString;
            }
        }

        /// <summary>
        /// 对字符串进行解压缩
        /// </summary>
        /// <param name="str">待解压缩的字符串</param>
        /// <returns>解压缩后的字符串</returns>
        public static string DecompressString(string str)
        {
            string decompressString = str;
            try
            {
                byte[] decompressBeforeByte = Convert.FromBase64String(str);
                byte[] decompressAfterByte = Utils.Decompress(decompressBeforeByte);

                return Encoding.UTF8.GetString(decompressAfterByte, 0, decompressAfterByte.Length);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return decompressString;
            }
        }
        #endregion

        #region 字符串值处理
        public static string DealString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                if (str == "null")
                {
                    return null;
                }
                return str;
            }
        }
        #endregion

        #region 16进制相关操作
        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary> 
        /// 从汉字转换到16进制 
        /// </summary> 
        /// <param name="s"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <param name="fenge">是否每字符用逗号分隔</param> 
        /// <returns></returns> 
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格 
                //throw new ArgumentException("s is not valid chinese string!"); 
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        ///<summary> 
        /// 从16进制转换成汉字 
        /// </summary> 
        /// <param name="hex"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <returns></returns> 
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格 
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes, 0, bytes.Length);
        }

        #endregion
    }
}
