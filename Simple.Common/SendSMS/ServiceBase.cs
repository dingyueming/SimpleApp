using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Simple.Common.SendSMS {
    public class ServiceBase {
        private readonly string _serviceUrl;
        protected ServiceBase (string serviceUrl) {
            _serviceUrl = serviceUrl;
        }

        protected object InvokeMethod (string methodName, params object[] methodPars) {
            Dictionary<string, string> pars = ParsToDic (_serviceUrl, methodName, methodPars);
            return QuerySoapWebService_1_1 (_serviceUrl, methodName, pars);
        }

        //缓存xmlNamespace，避免重复调用GetNamespace
        private static Dictionary<string, string> _xmlNamespaces = new Dictionary<string, string> ();
        //缓存方法列表
        private static Dictionary<string, List<MethodInfo>> _xmlMethodList = new Dictionary<string, List<MethodInfo>> ();
        /// <summary>    
        /// 需要WebService支持Post调用    
        /// </summary>    
        public static XmlDocument QueryPostWebService (String URL, String MethodName, Dictionary<string, string> Pars) {
            if (!CheckMethod (URL, MethodName)) return null;
            HttpClient client = new HttpClient ();
            SetHttpClient (client);
            //client.DefaultRequestHeaders.Add("Content-Type","application/x-www-form-urlencoded");
            HttpContent content = new FormUrlEncodedContent (Pars);
            HttpResponseMessage responseMessage = client.PostAsync (URL + "/" + MethodName, content).Result;
            if (responseMessage.IsSuccessStatusCode) {
                Stream stream = responseMessage.Content.ReadAsStreamAsync ().Result;
                return ReadXmlResponse (stream);
            } else {
                return null;
            }
        }

        /// <summary>    
        /// 需要WebService支持Get调用    
        /// </summary>    
        public static XmlDocument QueryGetWebService (String URL, String MethodName, Dictionary<string, string> Pars) {
            if (!CheckMethod (URL, MethodName)) return null;
            HttpClient client = new HttpClient ();
            SetHttpClient (client);
            HttpResponseMessage responseMessage = client.GetAsync (URL + "/" + MethodName + "?" + ParsToString (Pars)).Result;
            if (responseMessage.IsSuccessStatusCode) {
                Stream stream = responseMessage.Content.ReadAsStreamAsync ().Result;
                return ReadXmlResponse (stream);
            } else
                return null;
        }

        /// <summary>    
        /// 通用WebService调用(Soap1.1),参数Pars为String类型的参数名、参数值    
        /// </summary>    
        public static object QuerySoapWebService_1_1 (String URL, String MethodName, Dictionary<string, string> Pars) {
            string XmlNs = GetNamespace (URL);
            if (!CheckMethod (URL, MethodName)) return null;
            MethodInfo method = GetMethod (URL, MethodName);
            HttpClient client = new HttpClient ();
            SetHttpClient (client);
            HttpRequestMessage requestMessage = new HttpRequestMessage ();
            requestMessage.Method = HttpMethod.Post;
            requestMessage.RequestUri = new Uri (URL);
            requestMessage.Version = new Version (1, 1);
            byte[] data = EncodeParsToSoap (Pars, XmlNs, MethodName);
            HttpContent content = new ByteArrayContent (data);
            content.Headers.ContentType = new MediaTypeHeaderValue ("text/xml");
            content.Headers.ContentType.CharSet = "utf-8";
            content.Headers.Add ("SOAPAction", XmlNs + MethodName);
            requestMessage.Content = content;
            HttpResponseMessage responseMessage = client.SendAsync (requestMessage).Result;
            if (responseMessage.IsSuccessStatusCode) {
                Stream stream = responseMessage.Content.ReadAsStreamAsync ().Result;
                return XmlDocToSoapReturnValue (ReadXmlResponse (stream), XmlNs, method);
            }
            return null;
        }

        private static bool CheckMethod (string url, string methodName) {
            var tmp = GetMethod (url, methodName);
            return tmp != null;
        }

        private static MethodInfo GetMethod (string url, string methodName) {
            List<MethodInfo> methodList = GetMethodList (url);
            return methodList.FirstOrDefault (m => m.Name == methodName);
        }

        private static List<MethodInfo> GetMethodList (string URL, XmlDocument doc = null) {
            if (!_xmlMethodList.ContainsKey (URL) || _xmlMethodList[URL] == null || _xmlMethodList[URL].Count == 0) {
                if (doc == null) {
                    HttpClient client = new HttpClient ();
                    SetHttpClient (client);
                    string wsdlXml = client.GetStringAsync (URL + "?WSDL").Result;
                    doc = new XmlDocument ();
                    doc.LoadXml (wsdlXml);
                }

                List<MethodInfo> methodList = new List<MethodInfo> ();
                List<XmlNode> returnList = new List<XmlNode> ();
                XmlNodeList nodes = doc.GetElementsByTagName ("wsdl:types") [0].FirstChild.ChildNodes;
                for (int i = 0; i < nodes.Count; i++) {
                    XmlNode node = nodes[i];
                    if (node.Name == "s:element" && node.Attributes.Count == 1) {
                        string name = node.Attributes["name"].Value;
                        if (!name.EndsWith ("Response")) {
                            MethodInfo info = new MethodInfo ();
                            info.Name = name;
                            if (node.ChildNodes.Count > 0) {
                                if (node.FirstChild.ChildNodes.Count > 0) {
                                    if (node.FirstChild.FirstChild.ChildNodes.Count > 0) {
                                        List<ParameterInfo> parList = new List<ParameterInfo> ();
                                        foreach (XmlNode item in node.FirstChild.FirstChild.ChildNodes) {
                                            ParameterInfo par = new ParameterInfo ();
                                            par.Name = item.Attributes["name"].Value;
                                            par.Type = item.Attributes["type"].Value;
                                            par.minOccurs = Convert.ToInt32 (item.Attributes["minOccurs"].Value);
                                            par.maxOccurs = Convert.ToInt32 (item.Attributes["maxOccurs"].Value);
                                            parList.Add (par);
                                        }
                                        info.ParameterList = parList;
                                    }
                                }
                            }
                            methodList.Add (info);
                        } else {
                            returnList.Add (node);
                        }
                    }
                }
                foreach (XmlNode returnNode in returnList) {
                    ParameterInfo returnInfo = new ParameterInfo ();
                    string methodName = returnNode.Attributes["name"].Value.Replace ("Response", "");
                    XmlNode tmp = returnNode.FirstChild?.FirstChild?.FirstChild;
                    if (tmp == null) continue;
                    returnInfo.minOccurs = Convert.ToInt32 (tmp.Attributes["minOccurs"].Value);
                    returnInfo.maxOccurs = Convert.ToInt32 (tmp.Attributes["maxOccurs"].Value);
                    returnInfo.Name = tmp.Attributes["name"].Value;
                    returnInfo.Type = tmp.Attributes["type"].Value;
                    MethodInfo method = methodList.FirstOrDefault (m => m.Name == methodName);
                    if (method != null)
                        method.ReturnValue = returnInfo;
                }
                _xmlMethodList[URL] = methodList;
                return methodList;
            } else {
                return _xmlMethodList[URL];
            }
        }
        private static string GetNamespace (String URL) {
            if (_xmlNamespaces.ContainsKey (URL) && !string.IsNullOrWhiteSpace (_xmlNamespaces[URL])) {
                return _xmlNamespaces[URL];
            }

            HttpClient client = new HttpClient ();
            SetHttpClient (client);
            string wsdlXml = client.GetStringAsync (URL + "?WSDL").Result;
            XmlDocument doc = new XmlDocument ();
            doc.LoadXml (wsdlXml);
            string xmlNs = doc.LastChild.Attributes["targetNamespace"].Value?.TrimEnd ('/');
            if (xmlNs != null)
                xmlNs += "/";
            _xmlNamespaces[URL] = xmlNs; //加入缓存，提高效率

            GetMethodList (URL, doc);

            return xmlNs;
        }
        private static byte[] EncodeParsToSoap (Dictionary<string, string> Pars, String XmlNs, String MethodName) {
            string rooXmlStr = "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>";
            XmlDocument doc = new XmlDocument ();
            doc.LoadXml (rooXmlStr);
            AddDelaration (doc);
            XmlNamespaceManager xnm = new XmlNamespaceManager (doc.NameTable);
            xnm.AddNamespace ("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlNode root = doc.SelectSingleNode ("soap:Envelope", xnm);
            XmlElement body = doc.CreateElement ("soap", "Body", xnm.LookupNamespace ("soap"));
            root.AppendChild (body);
            XmlElement method = doc.CreateElement (MethodName, XmlNs);
            body.AppendChild (method);
            foreach (var item in Pars) {
                XmlElement ele = doc.CreateElement (item.Key, XmlNs);
                XmlText text = doc.CreateTextNode (item.Value);
                ele.AppendChild (text);
                method.AppendChild (ele);
            }

            MemoryStream ms = new MemoryStream ();
            doc.Save (ms);
            return ms.ToArray ();
        }

        private static void SetHttpClient (HttpClient client) {
            client.Timeout = new TimeSpan (0, 0, 10);
            //HttpRequestHeaders headers= client.DefaultRequestHeaders;
            //headers.Add("Content-Type","text/xml; charset=utf-8");
        }

        private static Dictionary<string, string> ParsToDic (string url, string methodName, params object[] methodPar) {
            Dictionary<string, string> dictionary = new Dictionary<string, string> ();
            if (methodPar.Length == 0) return dictionary;

            GetNamespace (url);
            List<MethodInfo> list = GetMethodList (url);
            MethodInfo method = list.FirstOrDefault (m => m.Name == methodName);
            if (method == null) return null;

            for (int i = 0; i < method.ParameterList.Count; i++) {
                ParameterInfo p = method.ParameterList[i];
                object val = null;
                if (i < methodPar.Length)
                    val = methodPar[i];
                dictionary.Add (p.Name, Convert2ParString (p.Type, val));
            }
            return dictionary;
        }

        private static string Convert2ParString (string type, object val) {
            if (type == "s:string") {
                return val?.ToString ();
            } else if (type == "s:base64Binary") {
                if (val == null) return null;
                if (val is byte[]) {
                    return Convert.ToBase64String ((byte[]) val);
                } else {
                    return null;
                }
            } else {
                throw new Exception ("未识别的类型");
            }
        }
        private static String ParsToString (Dictionary<string, string> Pars) {
            StringBuilder sb = new StringBuilder ();
            foreach (string k in Pars.Keys) {
                if (sb.Length > 0) {
                    sb.Append ("&");
                }
                sb.Append (Uri.EscapeDataString (k) + "=" + Uri.EscapeDataString (Pars[k]));
            }
            return sb.ToString ();
        }

        private static XmlDocument ReadXmlResponse (Stream stream) {
            StreamReader sr = new StreamReader (stream, Encoding.UTF8);
            String retXml = sr.ReadToEnd ();
            sr.Dispose ();
            XmlDocument doc = new XmlDocument ();
            doc.LoadXml (retXml);
            return doc;
        }

        private static object XmlDocToSoapReturnValue (XmlDocument doc, string xmlNs, MethodInfo methodInfo) {
            XmlElement node = (XmlElement) doc.GetElementsByTagName (methodInfo.Name + "Response", xmlNs) [0];
            node = (XmlElement) node.GetElementsByTagName (methodInfo.ReturnValue.Name, xmlNs) [0];
            return Convert2ReturnValue (node.FirstChild.Value, methodInfo.ReturnValue.Type);
        }

        private static object Convert2ReturnValue (string xmlVal, string xmlType) {
            if (xmlType == "s:string") {
                return xmlVal;
            } else {
                throw new Exception ("未知的返回类型");
            }
        }
        private static void AddDelaration (XmlDocument doc) {
            XmlDeclaration decl = doc.CreateXmlDeclaration ("1.0", "utf-8", null);
            doc.InsertBefore (decl, doc.DocumentElement);
        }
    }

    /// <summary>
    /// 方法信息类
    /// </summary>
    class MethodInfo {
        /// <summary>
        /// 方法名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        /// <returns></returns>
        public List<ParameterInfo> ParameterList { get; set; }
        /// <summary>
        /// 返回值信息
        /// </summary>
        /// <returns></returns>
        public ParameterInfo ReturnValue { get; set; }

        public override string ToString () {
            return this.Name;
        }
    }
    /// <summary>
    /// 方法参数信息
    /// </summary>
    class ParameterInfo {
        /// <summary>
        /// 参数名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        /// <returns></returns>
        public string Type { get; set; }
        public int minOccurs { get; set; }
        public int maxOccurs { get; set; }

        public override string ToString () {
            return this.Name;
        }
    }
}