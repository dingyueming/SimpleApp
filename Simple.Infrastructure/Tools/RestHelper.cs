using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Simple.Infrastructure.Tools
{
    public class RestHelper<T> where T : class, new()
    {
        public async static Task<T> Get(string url, object pars = null)
        {
            var type = Method.GET;
            IRestResponse<T> reval = await GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public async static Task<T> Post(string url, object pars = null)
        {
            var type = Method.POST;
            IRestResponse<T> reval = await GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public async static Task<T> Delete(string url, object pars = null)
        {
            var type = Method.DELETE;
            IRestResponse<T> reval = await GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public async static Task<T> Put(string url, object pars = null)
        {
            var type = Method.PUT;
            IRestResponse<T> reval = await GetApiInfo(url, pars, type);
            return reval.Data;
        }

        private async static Task<IRestResponse<T>> GetApiInfo(string url, object pars, Method type)
        {
            var request = new RestRequest(type);
            if (pars != null)
            {
                request.AddParameter("text/plain", pars, ParameterType.RequestBody);
            }
            var client = new RestClient(url);
            client.CookieContainer = new System.Net.CookieContainer();
            client.Encoding = Encoding.UTF8;
            IRestResponse<T> reval = await client.ExecuteAsync<T>(request);
            if (reval.ErrorException != null)
            {
                throw new Exception("请求出错");
            }
            return reval;
        }
    }
}
