// <copyright file="Message.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.SignalR
{
    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="timeStamp">Time stamp of the message.</param>
        /// <param name="user">User name.</param>
        /// <param name="message">A message.</param>
        public Message(string timeStamp, string user, string message)
        {
            this.TimeStamp = timeStamp;
            this.User = user;
            this.MessageText = message;
        }

        /// <summary>
        /// Gets or sets the time stamp of the message.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public string MessageText { get; set; }
    }
}
