﻿using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.Server.Hubs
{

        public class ChatHub : Hub
        {
            public async Task SendMessage(string user, string message)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }
}
