using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Simple.Infrastructure.Tools;
using Simple.Web.Extension.MapApi;
using Simple.Web.Models;
using Simple.Web.Models.CmdInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simple.Web.Other
{
    [Authorize]
    public class MapHub : Hub
    {
        private readonly RedisHelper redisHelper;
        private readonly DigitalQueueHelper digitalQueueHelper;
        private readonly RtMapService rtMapService;
        /// <summary>
        /// 客户端连接id
        /// </summary>
        public List<string> ConnIds { get; set; }

        public List<CmdByUser> CmdByUsers { get; set; }

        public MapHub(RedisHelper redisHelper, DigitalQueueHelper digitalQueueHelper, RtMapService rtMapService)
        {
            this.digitalQueueHelper = digitalQueueHelper;
            this.redisHelper = redisHelper;
            this.rtMapService = rtMapService;
            ConnIds = new List<string>();
            CmdByUsers = new List<CmdByUser>();
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <returns></returns>
        public async Task GetData()
        {
            try
            {
                while (true)
                {
                    //位置信息
                    var gpsData = redisHelper.GetAndRemoveListValue<DataCenterModel>("TRACK");
                    //短报文
                    var msgData = redisHelper.GetAndRemoveListValue<UpMsg>("UPMSG");
                    //命令反馈
                    var ackData = redisHelper.GetAndRemoveListValue<BaseCommand<CmdResponse>>("CMDACK");
                    //路线规划
                    var directionData = redisHelper.GetAndRemoveListValue<DirectionModel>("DIRECTION");
                    //有客户端连接时
                    if (Clients != null && ConnIds.Count > 0)
                    {
                        if (gpsData != null)
                        {
                            var connClients = Clients.Clients(ConnIds);
                            await connClients.SendAsync("UpdateMapData", gpsData);
                        }

                        if (msgData != null)
                        {
                            var connClients = Clients.Clients(ConnIds);
                            await connClients.SendAsync("UpdateMsgData", msgData);
                        }

                        if (ackData != null)
                        {
                            var connectionId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
                            var listCmd = CmdByUsers.Where(x => x.USERID == ackData.Head.USERID && x.ConnId == connectionId && x.COMMAND_ID == ackData.Content.Cmd).ToList();
                            //移除命令
                            foreach (var item in listCmd)
                            {
                                CmdByUsers.Remove(item);
                            }
                            var userCmd = listCmd.FirstOrDefault(x => x.Equals(ackData));

                            if (userCmd != null)
                            {
                                var clientId = ConnIds.FirstOrDefault(x => x == userCmd.ConnId);

                                //推送给客户端
                                var connClients = Clients.Clients(clientId);
                                await connClients.SendAsync("ShowCommandMsg", ackData.Content);
                            }
                        }

                        if (directionData != null)
                        {
                            var path = new List<double[]>();
                            var polyline = await rtMapService.GetDrivingLine(directionData.Origin, directionData.Destination);
                            var strPathList = polyline.Split(';').ToList();
                            foreach (var item in strPathList)
                            {
                                var arr = item.Split(',');
                                if (arr.Length == 2)
                                {
                                    var p = new double[] { double.Parse(arr[0]), double.Parse(arr[1]) };
                                    path.Add(p);
                                }
                            }
                            var connClients = Clients.Clients(ConnIds);
                            await connClients.SendAsync("DrawDirLine", path, directionData);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var connClients = Clients.Clients(ConnIds);
                if (connClients != null)
                {
                    await connClients.SendAsync("MapHubException", e.Message);
                }
                await GetData();
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
            var connectionId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
            CmdByUsers.Add(new CmdByUser() { USERID = userId, ConnId = connectionId, CMD_SEQ = outPutModel.Head.CMD_SEQ, COMMAND_ID = outPutModel.Head.COMMAND_ID, });
            await Task.Run(() =>
            {
                redisHelper.SetListValue("CMD", outPutModel);
            });
        }

        /// <summary>
        /// 设置回传间隔
        /// </summary>
        /// <returns></returns>
        public async Task SetReturnInterval(string mac, int mtype, int ctype, int minutes)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var outPutModel = new BaseCommand<SetReturnInterval>()
            {
                Mac = mac,
                Head = new CommandHead()
                {
                    COMMAND_ID = 6,
                    USERID = userId,
                    MOBILE_TYPE = mtype,
                    CI_SERVERNO = ctype,
                    CMD_SEQ = digitalQueueHelper.NextNumber()
                },
                Content = new SetReturnInterval()
                {
                    Number = 1,
                    Para = minutes,
                    Type = 2
                }
            };
            var connectionId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
            CmdByUsers.Add(new CmdByUser() { USERID = userId, ConnId = connectionId, CMD_SEQ = outPutModel.Head.CMD_SEQ, COMMAND_ID = outPutModel.Head.COMMAND_ID, });
            await Task.Run(() =>
            {
                redisHelper.SetListValue("CMD", outPutModel);
            });
        }

        /// <summary>
        /// 下发短报文
        /// </summary>
        /// <returns></returns>
        public async Task Xfdbwen(string mac, int mtype, int ctype, string msg)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var outPutModel = new BaseCommand<UpMsgContent>()
            {
                Mac = mac,
                Head = new CommandHead()
                {
                    COMMAND_ID = 4,
                    USERID = userId,
                    MOBILE_TYPE = mtype,
                    CI_SERVERNO = ctype,
                    CMD_SEQ = digitalQueueHelper.NextNumber()
                },
                Content = new UpMsgContent()
                {
                    Msg = msg,
                    Type = 1
                }
            };
            var connectionId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
            CmdByUsers.Add(new CmdByUser() { USERID = userId, ConnId = connectionId, CMD_SEQ = outPutModel.Head.CMD_SEQ, COMMAND_ID = outPutModel.Head.COMMAND_ID, });
            await Task.Run(() =>
            {
                redisHelper.SetListValue("CMD", outPutModel);
            });
        }

        /// <summary>
        /// 下发导航点
        /// </summary>
        /// <returns></returns>
        public async Task Xfdhd(string mac, int mtype, int ctype, double longitude, double laitude, string name, int path)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var outPutModel = new BaseCommand<SetPoint>()
            {
                Mac = mac,
                Head = new CommandHead()
                {
                    COMMAND_ID = 36,
                    USERID = userId,
                    MOBILE_TYPE = mtype,
                    CI_SERVERNO = ctype,
                    CMD_SEQ = digitalQueueHelper.NextNumber()
                },
                Content = new SetPoint()
                {
                    Longitude = longitude,
                    Latitude = laitude,
                    Map_Type = 0,
                    Path = path,
                    Name = name
                }
            };
            var connectionId = Context.User.FindFirst(ClaimTypes.PrimarySid).Value;
            CmdByUsers.Add(new CmdByUser() { USERID = userId, ConnId = connectionId, CMD_SEQ = outPutModel.Head.CMD_SEQ, COMMAND_ID = outPutModel.Head.COMMAND_ID, });
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
            Context.User.AddIdentity(new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.PrimarySid,Context.ConnectionId),
                }));

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
