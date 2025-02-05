using Microsoft.AspNetCore.SignalR;
using SadSchool.Contracts;

namespace SadSchool.Services.SignalR
{
    public class SignalRChatHub : Hub, ISignalRChatHub
    {
        public async Task SendMessage(string user, string message)
        {
            var timestamp = DateTime.Now.ToString();
            await Clients.All.SendAsync("ReceiveMessage", user, message, timestamp);
        }
    }
}
