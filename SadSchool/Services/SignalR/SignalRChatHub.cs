using Microsoft.AspNetCore.SignalR;
using SadSchool.Contracts;

namespace SadSchool.Services.SignalR
{
    public class SignalRChatHub : Hub, ISignalRChatHub
    {
        private static readonly ChatHistory _chatHistory = new();

        public override async Task OnConnectedAsync()
        {
            // Send chat history to the newly connected client
            await Clients.Caller.SendAsync("LoadChatHistory", _chatHistory.GetMessages());
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string messageText)
        {
            var timestamp = DateTime.Now.ToString();
            var message = new Message(timestamp, user, messageText);
            _chatHistory.AddMessage(message);

            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task ClearChat()
        {
            _chatHistory.Clear();
            var message = new Message(DateTime.Now.ToString(), "System", "Chat has been cleared.");
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
