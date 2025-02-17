namespace SadSchool.Services.SignalR
{
    public class ChatHistory
    {
        private readonly int _maxMessages = 50;
        private readonly Queue<Message> _messages = new();

        public void AddMessage(Message message)
        {
            if (_messages.Count >= _maxMessages)
            {
                _messages.Dequeue(); // Remove oldest message
            }
            _messages.Enqueue(message);
        }

        public void Clear() => _messages.Clear();

        public List<Message> GetMessages() => _messages.ToList();
    }

    public class Message
    {
        public Message(string timeStamp, string user, string message)
        {
            TimeStamp = timeStamp;
            User = user;
            MessageText = message;
        }

        public string TimeStamp { get; set; }

        public string User { get; set; }
        
        public string MessageText { get; set; }
    }
}
