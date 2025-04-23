// <copyright file="ChatHistory.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.SignalR
{
    /// <summary>
    /// Class for managing chat history.
    /// </summary>
    public class ChatHistory
    {
        private readonly int maxMessages = 50;
        private readonly Queue<Message> messages = new();

        /// <summary>
        /// Adds a message to the chat history.
        /// </summary>
        /// <param name="message">A message to add.</param>
        public void AddMessage(Message message)
        {
            if (this.messages.Count >= this.maxMessages)
            {
                this.messages.Dequeue(); // Remove oldest message
            }

            this.messages.Enqueue(message);
        }

        /// <summary>
        /// Clears the chat history.
        /// </summary>
        public void Clear() => this.messages.Clear();

        /// <summary>
        /// Gets the chat history messages.
        /// </summary>
        /// <returns>Retrieves all chat history messages.</returns>
        public List<Message> GetMessages() => this.messages.ToList();
    }
}
