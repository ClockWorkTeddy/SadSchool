using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBlackboardHub
    {
        Task SendDrawingData(int prevX, int prevY, int currentX, int currentY, string color, int lineWidth);

        Task ClearBoard();
    }
}
