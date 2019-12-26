using System;
using System.IO;
using System.Net;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace Simple.Common
{
    public class WebTools
    {
        /// <summary>
        /// WebAPI路径
        /// </summary>
        public static string apiUrl;
        /// <summary>
        /// 特殊应用WebAPI路径
        /// </summary>
        public static string apiUrl2;
        /// <summary>
        /// 获得API路径方法
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Uri GetApiUrl(string controller, string id = null)
        {
            if (id == null || string.IsNullOrWhiteSpace(id))
                return new Uri(apiUrl + "/api/" + controller);
            else
                return new Uri(apiUrl + "/api/" + controller + "?id=" + UrlEncode(id));
        }
        /// <summary>
        /// 获得API路径方法
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Uri GetApiUrl2(string controller, string id = null)
        {
            if (id == null || string.IsNullOrWhiteSpace(id))
                return new Uri(apiUrl2 + "/api/" + controller);
            else
                return new Uri(apiUrl2 + "/api/" + controller + "?id=" + UrlEncode(id));
        }

        /// <summary>
        /// API访问用户名
        /// </summary>
        public static string apiAuth;
        /// <summary>
        /// API访问密匙
        /// </summary>
        public static string apiKey;

        private static bool inited = false;

        /// <summary>
        /// 初始化运行环境
        /// </summary>
        public static void Init(string apiUrl, string apiAuth, string apiKey)
        {
            WebTools.apiUrl = apiUrl;
            WebTools.apiAuth = apiAuth;
            WebTools.apiKey = apiKey;

            inited = true;
        }

        /// <summary>
        /// 从返回结果中读取Stream
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static Stream GetStreamFromResponse(WebResponse response)
        {
            return response.GetResponseStream();
        }

        /// <summary>
        /// 从返回结果中读取String
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string GetStringFromResponse(WebResponse response)
        {
            Stream stream = null;
            StreamReader sr = null;
            try
            {
                stream = GetStreamFromResponse(response);
                sr = new StreamReader(stream, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                sr?.Dispose();
                stream?.Dispose();
            }
        }

        /// <summary>
        /// 从返回结果中读取ReturnResult
        /// </summary>
        /// <returns></returns>
        private static ReturnResult GetResultFromResponse(WebResponse response)
        {
            if (response == null) return ReturnResult.NewWithMsg("服务器返回为空");
            Stream stream = null;
            StreamReader sr = null;
            string rstr = null;
            try
            {
                stream = GetStreamFromResponse(response);
                sr = new StreamReader(stream, Encoding.UTF8);
                rstr = sr.ReadToEnd();
                ReturnResult rt = JsonConvert.DeserializeObject<ReturnResult>(rstr);
                if (rt.code == -1 && string.IsNullOrEmpty(rt.msg))
                    rt.tag = rstr;
                return rt;
            }
            catch
            {
                ReturnResult rt = ReturnResult.NewWithMsg("服务器返回数据格式错误，可能原因：不是ReturnResult对象");
                rt.tag = rstr;
                return rt;
            }
            finally
            {
                sr?.Dispose();
                stream?.Dispose();
                response.Dispose();
            }
        }

        private static void SetBasicAuthorization(HttpWebRequest webRequest)
        {
            if (inited)
            {
                webRequest.Accept = MediaType.APPLICATION_JSON;

                //获得用户名密码的Base64编码
                string code = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiAuth, apiKey)));
                webRequest.Headers["Authorization"] = "Basic " + code;
            }

            //跨域Cookies 解决
            if (webRequest.CookieContainer != null)
            {
                string cookieHeader = webRequest.CookieContainer.GetCookieHeader(webRequest.RequestUri);
                if (!string.IsNullOrEmpty(cookieHeader))
                    webRequest.Headers["Cookie"] = cookieHeader;
                else
                {
                    //webRequest.Headers["Cookie"];
                }
            }
        }

        /// <summary>
        /// 向服务端POST请求数据
        /// </summary>
        public static void POST(object data, Action<bool, WebExceptionStatus, ReturnResult> resultFunc, Uri url)
        {
            if (!inited) throw new Exception("WebTools未初始化");
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "POST";
            SetBasicAuthorization(webRequest);
            webRequest.ContentType = MediaType.APPLICATION_JSON;

            webRequest.BeginGetRequestStream(x =>
            {
                StreamWriter sw = new StreamWriter(webRequest.EndGetRequestStream(x), Encoding.UTF8);
                sw.Write(JsonConvert.SerializeObject(data)); //写入数据
                sw.Flush();
                sw.Dispose();
                webRequest.BeginGetResponse(y =>
                {
                    WebResponse response = null;
                    bool suc = false;
                    WebExceptionStatus status = WebExceptionStatus.Success;
                    ReturnResult result = null;
                    try
                    {
                        response = webRequest.EndGetResponse(y);
                        suc = true;
                    }
                    catch (WebException ex)
                    {
                        status = ex.Status;
                    }

                    result = GetResultFromResponse(response);

                    if (resultFunc != null)
                    {
                        resultFunc(suc, status, result); //调用回调方法
                    }

                    result = null;
                }, null);
            }, null);
        }
        /// <summary>
        /// 向服务端POST请求数据
        /// </summary>
        public static void POST(object data, Action<bool, WebExceptionStatus, ReturnResult> resultFunc, string controller)
        {
            POST(data, resultFunc, GetApiUrl(controller));
        }

        /// <summary>
        /// 向服务端PUT请求数据
        /// </summary>
        public static void PUT(object data, Action<bool, WebExceptionStatus, ReturnResult> resultFunc, Uri url)
        {
            if (!inited) throw new Exception("WebTools未初始化");
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "PUT";
            SetBasicAuthorization(webRequest);
            webRequest.ContentType = MediaType.APPLICATION_JSON;

            webRequest.BeginGetRequestStream(x =>
            {
                StreamWriter sw = new StreamWriter(webRequest.EndGetRequestStream(x), Encoding.UTF8);
                sw.Write(JsonConvert.SerializeObject(data));    //写入数据
                sw.Flush();
                sw.Dispose();
                webRequest.BeginGetResponse(y =>
                {
                    WebResponse response = null;
                    bool suc = false;
                    WebExceptionStatus status = WebExceptionStatus.Success;
                    ReturnResult result = null;
                    try
                    {
                        response = webRequest.EndGetResponse(y);
                        suc = true;
                    }
                    catch (WebException ex)
                    {
                        status = ex.Status;
                    }
                    result = GetResultFromResponse(response);

                    if (resultFunc != null)
                    {
                        resultFunc(suc, status, result);    //调用回调方法
                    }

                    result = null;
                }, null);
            }, null);
        }
        /// <summary>
        /// 向服务端POST请求数据
        /// </summary>
        public static void PUT(object data, Action<bool, WebExceptionStatus, ReturnResult> resultFunc, string controller, string id)
        {
            PUT(data, resultFunc, GetApiUrl(controller, id));
        }

        /// <summary>
        /// 向服务端GET请求字符串
        /// </summary>
        /// <param name="resultFunc"></param>
        /// <param name="url"></param>
        public static void GetString(Action<WebExceptionStatus, string> resultFunc, Uri url)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";
            SetBasicAuthorization(webRequest);

            webRequest.BeginGetResponse(y =>
            {
                WebResponse response = null;
                WebExceptionStatus status = WebExceptionStatus.Success;
                string result = null;
                try
                {
                    response = webRequest.EndGetResponse(y);
                    result = GetStringFromResponse(response);
                }
                catch (WebException ex)
                {
                    status = ex.Status;
                }

                resultFunc?.Invoke(status, result);    //调用回调方法
            }, null);
        }

        /// <summary>
        /// 向服务端GET请求数据
        /// </summary>
        public static void GET(Action<bool, WebExceptionStatus, ReturnResult> resultFunc, Uri url)
        {
            if (!inited) throw new Exception("WebTools未初始化");
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";
            SetBasicAuthorization(webRequest);

            webRequest.BeginGetResponse(y =>
            {
                WebResponse response = null;
                bool suc = false;
                WebExceptionStatus status = WebExceptionStatus.Success;
                ReturnResult result = null;
                try
                {
                    response = webRequest.EndGetResponse(y);
                    suc = true;
                }
                catch (WebException ex)
                {
                    status = ex.Status;
                }

                result = GetResultFromResponse(response);

                if (resultFunc != null)
                {
                    resultFunc(suc, status, result);    //调用回调方法
                }

                result = null;
            }, null);
        }

        /// <summary>
        /// 向服务端GET请求数据
        /// </summary>
        public static void GET(Action<bool, WebExceptionStatus, ReturnResult> resultFunc, string controller,
            string id = null)
        {
            GET(resultFunc, GetApiUrl(controller, id));
        }


        /// <summary>
        /// 向服务端DELETE请求数据
        /// </summary>
        public static void DELETE(Action<bool, WebExceptionStatus, ReturnResult> resultFunc, Uri url)
        {
            if (!inited) throw new Exception("WebTools未初始化");
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "DELETE";
            SetBasicAuthorization(webRequest);

            webRequest.BeginGetResponse(y =>
            {
                WebResponse response = null;
                bool suc = false;
                WebExceptionStatus status = WebExceptionStatus.Success;
                ReturnResult result = null;
                try
                {
                    response = webRequest.EndGetResponse(y);
                    suc = true;
                }
                catch (WebException ex)
                {
                    status = ex.Status;
                }

                result = GetResultFromResponse(response);

                if (resultFunc != null)
                {
                    resultFunc(suc, status, result);    //调用回调方法
                }
                result = null;
            }, null);
        }

        /// <summary>
        /// 向服务端DELETE请求数据
        /// </summary>
        public static void DELETE(Action<bool, WebExceptionStatus, ReturnResult> resultFunc, string controller,
            string id)
        {
            DELETE(resultFunc, GetApiUrl(controller, id));
        }

        /// <summary>
        /// 判断请求结果是否成功
        /// </summary>
        /// <returns></returns>
        public static bool IsReturnSuc(bool b, WebExceptionStatus status, ReturnResult arg3)
        {
            if (arg3 == null) return false;
            return b & status == WebExceptionStatus.Success & arg3.code == 0;
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }

    public sealed class MediaType
    {
        /// <summary>
        /// "application/xml"
        /// </summary>
        public const string APPLICATION_XML = "application/xml";

        /// <summary>
        /// application/json
        /// </summary>
        public const string APPLICATION_JSON = "application/json";

        /// <summary>
        /// "application/x-www-form-urlencoded"
        /// </summary>
        public const string APPLICATION_FORM_URLENCODED = "application/x-www-form-urlencoded";

    }
}
