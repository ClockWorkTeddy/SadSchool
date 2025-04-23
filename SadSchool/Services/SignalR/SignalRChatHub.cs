// <copyright file="SignalRChatHub.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.SignalR
{
    using Microsoft.AspNetCore.SignalR;
    using SadSchool.Contracts;

    /// <summary>
    /// SignalR hub for chat functionality.
    /// </summary>
    public class SignalRChatHub : Hub, ISignalRChatHub
    {
        private static readonly ChatHistory ChatHistory = new();

        /// <summary>
        /// Called when a new client connects to the hub.
        /// </summary>
        /// <returns>Task (void).</returns>
        public override async Task OnConnectedAsync()
        {
            // Send chat history to the newly connected client
            await this.Clients.Caller.SendAsync("LoadChatHistory", ChatHistory.GetMessages());
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="user">Username.</param>
        /// <param name="messageText">Message.</param>
        /// <returns>Task (void).</returns>
        public async Task SendMessage(string user, string messageText)
        {
            var timestamp = DateTime.Now.ToString();
            var message = new Message(timestamp, user, messageText);
            ChatHistory.AddMessage(message);

            await this.Clients.All.SendAsync("ReceiveMessage", message);
        }

        /// <summary>
        /// Clears the chat history and notifies all clients.
        /// </summary>
        /// <returns>Task (void).</returns>
        public async Task ClearChat()
        {
            ChatHistory.Clear();
            var message = new Message(DateTime.Now.ToString(), "System", "Chat has been cleared.");
            await this.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
