using Microsoft.AspNetCore.SignalR;
using SadSchool.Contracts;

namespace SadSchool.Services
{
    public class SignalRChatHub : Hub, ISignalRChatHub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
