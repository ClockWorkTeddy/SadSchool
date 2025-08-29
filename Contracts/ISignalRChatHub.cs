namespace SadSchool.Contracts
{
    public interface ISignalRChatHub
    {
        Task SendMessage(string user, string messageText);
    }
}
