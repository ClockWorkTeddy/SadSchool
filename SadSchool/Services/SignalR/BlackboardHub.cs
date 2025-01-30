using Contracts;
using Microsoft.AspNetCore.SignalR;

public class BlackboardHub : Hub, IBlackboardHub
{
    public async Task SendDrawingData(int prevX, int prevY, int currentX, int currentY, string color, int lineWidth)
    {
        await Clients.Others.SendAsync("ReceiveDrawingData", prevX, prevY, currentX, currentY, color, lineWidth);
    }

    public async Task ClearBoard()
    {
        await Clients.All.SendAsync("ClearBoard");
    }
}