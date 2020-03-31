using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simple.ExEntity.Map;
using System.Collections.Generic;
using Simple.Web.Models;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;

namespace Simple.Web.Other
{
    [Authorize]
    public class MapHub : Hub
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFileProvider fileProvider;

        public MapHub(IConfiguration configuration)
        {
            fileProvider = new PhysicalFileProvider(configuration["DataCenterRoot"]);
            //this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <returns></returns>
        public async Task SendMsg()
        {
            while (true)
            {
                try
                {
                    var directoryContents = fileProvider.GetDirectoryContents("");
                    List<long> fileNameList = new List<long>();
                    foreach (var item in directoryContents)
                    {
                        //不是目录，并且存在
                        if (!item.IsDirectory && item.Exists && item.Name.ToLower().EndsWith("json"))
                        {
                            if (item.Name.Length != 23)
                            {
                                continue;
                            }
                            var msgTimeString = item.Name.Substring(1, 14);
                            var fileIndex = int.Parse(item.Name.Substring(15, 3));

                            var time = DateTime.ParseExact(msgTimeString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            //时间大于等于当前时间减一分钟的数据，才进行发送
                            if (time < DateTime.Now.AddMinutes(-1))
                            {
                                //File.Delete(item.PhysicalPath);
                                //continue;
                            }

                            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                            var unixTime = (long)(time - Jan1st1970).TotalSeconds;
                            fileNameList.Add(fileIndex + (unixTime * 1000));

                        }
                    }
                    //重新排序
                    var orderByFileName = fileNameList.OrderBy((x) => { return x; }).ToList();
                    foreach (var item in orderByFileName)
                    {
                        //时间戳
                        var timeStr = item / 1000;
                        //1970
                        DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                        //文件序号字符串
                        var indexFileStr = item.ToString().Substring(item.ToString().Length - 3, 3);
                        //完整的文件名为：
                        var fullFileName = $"r{time.AddSeconds(timeStr):yyyyMMddHHmmss}{indexFileStr}.json";
                        var fileInfo = directoryContents.FirstOrDefault(x => x.Name == fullFileName);
                        using (var stream = fileInfo.CreateReadStream())
                        {
                            byte[] result = new byte[stream.Length];
                            await stream.ReadAsync(result, 0, (int)stream.Length);
                            //注册Nuget包System.Text.Encoding.CodePages中的编码到.NET Core
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            var jsonStr = Encoding.GetEncoding("gb2312").GetString(result);
                            var listCneterModel = JsonConvert.DeserializeObject<List<DataCenterModel>>(jsonStr);
                            if (listCneterModel != null)
                            {
                                foreach (var dataCenter in listCneterModel)
                                {
                                    if (Clients != null)
                                    {
                                        await Clients.All.SendAsync("UpdateMapData", dataCenter);
                                    }
                                }
                            }
                        }
                        File.Delete(fileInfo.PhysicalPath);
                    }
                }
                catch (Exception)
                {
                    await SendMsg();
                }
            }
        }

        /// <summary>
        /// 新用户连接时
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 用户断开连接时
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
