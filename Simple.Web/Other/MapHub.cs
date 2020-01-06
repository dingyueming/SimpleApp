using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Simple.IApplication.SM;
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
        private static Dictionary<string, string> userConns = new Dictionary<string, string>();
        private static Dictionary<string, GnssSocket> dicClients = new Dictionary<string, GnssSocket>();
        private readonly GnssSocket gnssSocket;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        public MapHub(IHttpContextAccessor httpContextAccessor, IUserService userService, IConfiguration configuration, IHubContext<MapHub> hubContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;

            var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!dicClients.TryGetValue(userId, out GnssSocket client))
            {
                client = new GnssSocket(configuration, hubContext);
                dicClients.Add(userId, client);
            }
            gnssSocket = client;
        }

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("SendMessage", username, message);
        }

        public async Task Login()
        {
            var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = await userService.GetUserById(int.Parse(userId));
            if (user != null)
            {
                gnssSocket.GnssLogin(user.UsersName, user.Password);
            }
        }
        public override async Task OnConnectedAsync()
        {
            var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (userConns.TryGetValue(userId, out var connId))
            {
                if (Context.ConnectionId != connId)//同一个用户多登录
                {
                    await Clients.Client(connId).SendAsync("LoginOut");
                }
            }
            userConns[userId] = Context.ConnectionId;
            await base.OnConnectedAsync();
        }
    }
}
