using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Claims;

namespace Common.Web
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetString(this ISession session, string key, string value)
        {
            if (key != null && value != null)
            {
                session.Set(key, Encoding.UTF8.GetBytes(value));
            }
        }

        public static string GetString(this ISession session, string key)
        {
            byte[] values;
            bool suc = session.TryGetValue(key, out values);
            if (suc)
            {
                if (values == null) return null;
                return Encoding.UTF8.GetString(values);
            }
            else
            {
                return null;
            }
        }

        public static void SaveClaimsPrincipal(this ISession session, ClaimsPrincipal user)
        {
            if (user == null)
            {
                session.Remove("SUser");
                return;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in user.Claims)
            {
                dic.Add(item.Type, item.Value);
            }
            session.Set("SUser", dic);
        }

        public static ClaimsPrincipal LoadClaimsPrincipal(this ISession session)
        {
            var dic = session.Get<Dictionary<string, string>>("SUser");
            if (dic != null)
            {
                return new ClaimsPrincipal(new ClaimsIdentity(dic.Select(kv => new Claim(kv.Key, kv.Value)), "Basic"));
            }
            return null;
        }
    }
}
