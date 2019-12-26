using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Common
{
    public static class EncryptHelper
    {
        #region RSA加密算法

        /// <summary>
        /// 创建一个公钥
        /// </summary>
        /// <returns></returns>
        public static void RSA_Keys(out string strPublicKey, out string strPrivateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            strPublicKey = Convert.ToBase64String(provider.ExportCspBlob(false));
            strPrivateKey = Convert.ToBase64String(provider.ExportCspBlob(true));
            provider.Dispose();
        }

        /// <summary>
        /// RSA加密（自动生成公钥私钥）
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="strPublicKey">公钥</param>
        /// <param name="strPrivateKey">私钥</param>
        /// <returns></returns>
        public static string RSA_Encrypt(string text, out string strPublicKey, out string strPrivateKey)
        {
            strPrivateKey = strPublicKey = String.Empty;
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                strPublicKey = Convert.ToBase64String(provider.ExportCspBlob(false));
                strPrivateKey = Convert.ToBase64String(provider.ExportCspBlob(true));

                //OAEP padding is only available on Microsoft Windows XP or later. 
                byte[] bytesCypherText = provider.Encrypt(dataToEncrypt, false);
                provider.Dispose();
                string strCypherText = Convert.ToBase64String(bytesCypherText);
                return strCypherText;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// RSA加密（公钥加密）
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="strPublicKey">公钥</param>
        /// <returns></returns>
        public static string RSA_Encrypt(string text, string strPublicKey)
        {
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
                byte[] bytesPublicKey = Convert.FromBase64String(strPublicKey);

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.ImportCspBlob(bytesPublicKey);

                //OAEP padding is only available on Microsoft Windows XP or later. 
                byte[] bytesCypherText = provider.Encrypt(dataToEncrypt, false);
                provider.Dispose();

                string strCypherText = Convert.ToBase64String(bytesCypherText);
                return strCypherText;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="strCypherText">加密密文</param>
        /// <param name="strPrivateKey">私钥</param>
        /// <returns></returns>
        public static string RSA_Decrypt(string strCypherText, string strPrivateKey)
        {
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(strCypherText);
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                //RSA.ImportParameters(RSAKeyInfo);
                byte[] bytesPrivateKey = Convert.FromBase64String(strPrivateKey);
                provider.ImportCspBlob(bytesPrivateKey);

                //OAEP padding is only available on Microsoft Windows XP or later. 
                byte[] bytesText = provider.Decrypt(dataToDecrypt, false);
                provider.Dispose();

                string text = Encoding.UTF8.GetString(bytesText);
                return text;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region HMACMD5算法
        //数据签名
        public static byte[] SignData(string key, byte[] data)
        {
            byte[] bytesKey = Encoding.UTF8.GetBytes(key);
            HMAC alg = new HMACMD5();
            //设置密钥
            alg.Key = bytesKey;
            //计算哈希值
            byte[] hash = alg.ComputeHash(data);
            alg.Dispose();

            //返回具有签名的数据（哈希值+数组本身）
            return hash.Concat(data).ToArray();
        }
        //数据认证
        public static bool VerityData(string key, byte[] data)
        {
            byte[] bytesKey = Encoding.UTF8.GetBytes(key);
            HMAC alg = new HMACMD5();
            //提取收到的哈希值
            var receivedHash = data.Take(alg.HashSize >> 3);
            //提取数据本身
            var dataContent = data.Skip(alg.HashSize >> 3).ToArray();
            //设置密钥
            alg.Key = bytesKey;
            //计算数据哈希值和收到的哈希值
            var computedHash = alg.ComputeHash(dataContent);
            alg.Dispose();

            //如果相等则数据正确
            return receivedHash.SequenceEqual(computedHash);
        }

        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="key">加密密钥</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string text, string key)
        {
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
                byte[] bytesKey = Encoding.UTF8.GetBytes(key);
                HMAC hmac = new HMACMD5();
                hmac.Key = bytesKey;
                byte[] hash = hmac.ComputeHash(dataToEncrypt);
                hmac.Dispose();
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("X2")); // hex format
                }
                return result.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region 文件MD5计算
        /// <summary>  
        /// 根据文件路径计算文件Md5值  
        /// </summary>  
        /// <param name="path">文件地址</param>  
        /// <returns>MD5Hash</returns>  
        public static string GetMd5ByFilePath(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException($"{nameof(path)}<{path}>, 不存在");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            string result = GetMd5ByStream(fs);
            fs.Dispose();
            return result;
        }
        /// <summary>  
        /// 根据Stream计算MD5值 
        /// </summary>  
        /// <param name="stream">字节流</param>  
        /// <returns>MD5Hash</returns>  
        public static string GetMd5ByStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            MD5 md5Provider = MD5.Create();
            byte[] buffer = md5Provider.ComputeHash(stream);
            string result = BitConverter.ToString(buffer);
            result = result.Replace("-", "");
            md5Provider.Dispose();
            return result;
        }
        /// <summary>
        /// 根据数组计算MD5值
        /// </summary>
        /// <param name="buffer">数组Buffer</param>
        /// <returns></returns>
        public static string GetMd5ByBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            MD5 md5Provider = MD5.Create();
            byte[] buffer2 = md5Provider.ComputeHash(buffer);
            string result = BitConverter.ToString(buffer2);
            result = result.Replace("-", "");
            md5Provider.Dispose();
            return result;
        }
        #endregion
    }
}
