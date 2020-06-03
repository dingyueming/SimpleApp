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
using Simple.Web.Models.CmdInfo;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using System.Security.Claims;

namespace Simple.Web.Other
{
    [Authorize]
    public class MapHub : Hub
    {
        private readonly RedisHelper redisHelper;
        private readonly DigitalQueueHelper digitalQueueHelper;

        /// <summary>
        /// 客户端连接id
        /// </summary>
        public List<string> ConnIds { get; set; }

        public List<CmdByUser> CmdByUsers { get; set; }

        public MapHub(RedisHelper redisHelper, DigitalQueueHelper digitalQueueHelper)
        {
            this.digitalQueueHelper = digitalQueueHelper;
            this.redisHelper = redisHelper;
            ConnIds = new List<string>();
            CmdByUsers = new List<CmdByUser>();
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <returns></returns>
        public async Task GetData()
        {
            while (true)
            {
                if (Clients != null && ConnIds.Count > 0)
                {
                    //位置信息
                    var gpsData = redisHelper.GetAndRemoveListValue<DataCenterModel>("TRACK");
                    if (gpsData != null)
                    {
                        var connClients = Clients.Clients(ConnIds);
                        await connClients.SendAsync("UpdateMapData", gpsData);
                    }
                    //短报文
                    var msgData = redisHelper.GetAndRemoveListValue<UpMsg>("UPMSG");
                    if (msgData != null)
                    {
                        var connClients = Clients.Clients(ConnIds);
                        await connClients.SendAsync("UpdateMsgData", msgData);
                    }
                    //命令反馈
                    var ackData = redisHelper.GetAndRemoveListValue<BaseCommand<CmdResponse>>("CMDACK");
                    if (ackData != null)
                    {
                        var listCmd = CmdByUsers.Where(x => x.USERID == ackData.Head.USERID).ToList();
                        var needRemoveItme = new CmdByUser();
                        foreach (var item in listCmd)
                        {
                            if (item.Equals(ackData))
                            {
                                needRemoveItme = item;
                            }
                        }
                        listCmd.Remove(needRemoveItme);
                        var clientId = ConnIds.FirstOrDefault(x => x == needRemoveItme.ConnId);
                        if (ackData.Content.Status == 0)
                        {
                            var connClients = Clients.Clients(clientId);
                            await connClients.SendAsync("ShowCommandMsg", ackData.Content.ShowMsg);
                        }
                        else
                        {
                            //写入日志
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 位置查询
        /// </summary>
        /// <returns></returns>
        public async Task LocationQuery(string mac, int mtype, int ctype)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var outPutModel = new BaseCommand<LocationQuery>()
            {
                Mac = mac,
                Head = new CommandHead()
                {
                    COMMAND_ID = 5,
                    USERID = userId,
                    MOBILE_TYPE = mtype,
                    CI_SERVERNO = ctype,
                    CMD_SEQ = digitalQueueHelper.NextNumber()
                }
            };
            await Task.Run(() =>
            {
                redisHelper.SetListValue("CMD", outPutModel);
            });
        }

        /// <summary>
        /// 新用户连接时
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            ConnIds.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 用户断开连接时
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnIds.Any(x => x == Context.ConnectionId))
            {
                ConnIds.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
