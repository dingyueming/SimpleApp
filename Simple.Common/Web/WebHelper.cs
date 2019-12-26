using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Common.Web
{
    public class WebHelper
    {
        /// <summary>
        /// 获取真实IP
        /// </summary>
        /// <returns></returns>
        public static string GetRequestIP(HttpContext context)
        {
            var ipAddr = context.Connection.RemoteIpAddress;
            string result = "";
            if (IPAddress.IsLoopback(ipAddr))
            {
                result = "127.0.0.1";
            }
            else if (ipAddr.AddressFamily == AddressFamily.InterNetwork || ipAddr.AddressFamily == AddressFamily.InterNetworkV6)
            {
                result = ipAddr.ToString();
            }
            else
            {
                result = "unknown";
            }

            return result;
        }
    }
}
