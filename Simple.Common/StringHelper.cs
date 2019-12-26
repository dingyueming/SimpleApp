using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Simple.Common
{
    public static class StringHelper
    {
        #region 字符串与Base64之间的转换
        /// <summary>
        /// 将字符串转成Base64字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strEncoding">字符串编码</param>
        /// <returns></returns>
        public static string ConvertToBase64String(string str, Encoding strEncoding = null)
        {
            if (str == null) return null;
            if (strEncoding == null)
                strEncoding = Encoding.UTF8;
            byte[] strBytes = strEncoding.GetBytes(str);
            return Convert.ToBase64String(strBytes);
        }

        /// <summary>
        /// 将字符串转成Base64字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strEncoding">字符串编码</param>
        /// <returns></returns>
        public static string ToBase64String(this string str, Encoding strEncoding = null)
        {
            return ConvertToBase64String(str, strEncoding);
        }

        /// <summary>
        /// 将Base64字符串还原成原字符串
        /// </summary>
        /// <param name="base64Str">Base64字符串</param>
        /// <param name="strEncoding">字符串编码</param>
        /// <returns></returns>
        public static string ConvertFromBase64String(string base64Str, Encoding strEncoding = null)
        {
            if (base64Str == null) return null;
            if (strEncoding == null)
                strEncoding = Encoding.UTF8;
            try
            {
                byte[] strBytes = Convert.FromBase64String(base64Str);
                return strEncoding.GetString(strBytes, 0, strBytes.Length);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 将Base64字符串还原成原字符串
        /// </summary>
        /// <param name="base64Str">Base64字符串</param>
        /// <param name="strEncoding">字符串编码</param>
        /// <returns></returns>
        public static string FromBase64String(this string base64Str, Encoding strEncoding = null)
        {
            return ConvertFromBase64String(base64Str, strEncoding);
        }
        #endregion

        #region 数组相关操作
        ///<summary>
        /// GZip压缩数组
        ///</summary>
        ///<param name="data">需要压缩的数组</param>
        ///<returns>压缩后的数组</returns>
        public static byte[] GZipCompress(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress);
            gZipStream.Write(data, 0, data.Length);
            gZipStream.Dispose();
            byte[] bytes = stream.ToArray();
            stream.Dispose();
            return bytes;
        }

        /// <summary>
        /// GZip解压数组
        /// </summary>
        /// <param name="data">压缩的数组</param>
        /// <returns>解压后的数组</returns>
        public static byte[] GZipDecompress(byte[] data)
        {
            MemoryStream sourceStream = new MemoryStream(data);
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress);

            byte[] bytes = new byte[40960];
            int n;
            while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                stream.Write(bytes, 0, n);
            }
            gZipStream.Dispose();
            sourceStream.Dispose();
            byte[] resBytes = stream.ToArray();
            stream.Dispose();
            return resBytes;
        }
        #endregion

    }
}
