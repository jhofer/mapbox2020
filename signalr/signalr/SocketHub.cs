using System;
using Microsoft.AspNetCore.SignalR;

namespace signalr
{
    public class SocketHub: Hub
    {
      
        public void BroadcastMessage(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public void Echo( string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("Echo", message + " (echo from server)");
        }

    }
}
