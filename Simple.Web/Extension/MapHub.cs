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
using Simple.Infrastructure.Tools;
using Microsoft.AspNetCore.SignalR.Internal;

namespace Simple.Web.Other
{
    [Authorize]
    public class MapHub : Hub
    {
        private readonly RedisHelper redisHelper;

        /// <summary>
        /// 连接id
        /// </summary>
        private List<string> connIds;

        public MapHub(RedisHelper redisHelper)
        {
            this.redisHelper = redisHelper;
            connIds = new List<string>();
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <returns></returns>
        public async Task SendMsg()
        {
            while (true)
            {
                if (Clients != null)
                {
                    var data = redisHelper.GetAndRemoveListValue<DataCenterModel>("TRACK");
                    if (connIds.Count > 0 && data != null)
                    {
                        var connClients = Clients.Clients(connIds);
                        await connClients.SendAsync("UpdateMapData", data);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 新用户连接时
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            connIds.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 用户断开连接时
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (connIds.Any(x => x == Context.ConnectionId))
            {
                connIds.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
