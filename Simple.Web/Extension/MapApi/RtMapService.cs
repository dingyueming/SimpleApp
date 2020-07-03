using Microsoft.Extensions.Configuration;
using Simple.Infrastructure.Tools;
using Simple.Web.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using Org.BouncyCastle.Crypto.Modes;

namespace Simple.Web.Extension.MapApi
{
    public class RtMapService
    {
        private readonly IConfiguration configuration;

        public RtMapService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> GetDrivingLine(string origin, string destination)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                var url = $"https://restapi.amap.com/v3/direction/driving?key={configuration["MapKey"]}&origin={origin}&destination={destination}&size=3";
                var request = new RestRequest(Method.GET);
                var client = new RestClient(url);
                client.Encoding = Encoding.UTF8;
                IRestResponse reval = await client.ExecuteAsync(request);
                var model = JsonConvert.DeserializeObject<DrivingResModel>(reval.Content);

                if (model.Route != null)
                {
                    var paths = model.Route.Paths;
                    foreach (var path in paths)
                    {
                        var steps = path.Steps;
                        if (steps != null)
                        {
                            foreach (var step in steps)
                            {
                                str.Append(step.Polyline);
                                str.Append(";");
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                return "";
            }

            return str.ToString();
        }
    }
}
