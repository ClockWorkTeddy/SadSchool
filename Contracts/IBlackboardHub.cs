namespace SadSchool.Contracts
{
    public interface IBlackboardHub
    {
        Task SendDrawingData(int prevX, int prevY, int currentX, int currentY, string color, int lineWidth);

        Task ClearBoard();
    }
}
