// <copyright file="BlackboardHub.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.SignalR;

using Microsoft.AspNetCore.SignalR;
using SadSchool.Contracts;

/// <summary>
/// SignalR hub for blackboard functionality.
/// </summary>
public class BlackboardHub : Hub, IBlackboardHub
{
    /// <summary>
    /// Sends drawing data to all clients except the sender.
    /// </summary>
    /// <param name="prevX">Previous X value.</param>
    /// <param name="prevY">Previous Y value.</param>
    /// <param name="currentX">Current X value.</param>
    /// <param name="currentY">Current Y value.</param>
    /// <param name="color">The color of the line.</param>
    /// <param name="lineWidth">The width of the line.</param>
    /// <returns>Task (void).</returns>
    public async Task SendDrawingData(int prevX, int prevY, int currentX, int currentY, string color, int lineWidth)
    {
        await this.Clients.Others.SendAsync("ReceiveDrawingData", prevX, prevY, currentX, currentY, color, lineWidth);
    }

    /// <summary>
    /// Clears the blackboard and notifies all clients.
    /// </summary>
    /// <returns>Task (void).</returns>
    public async Task ClearBoard()
    {
        await this.Clients.All.SendAsync("ClearBoard");
    }
}