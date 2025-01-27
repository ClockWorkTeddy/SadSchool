using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadSchool.Contracts
{
    public interface ISignalRChatHub
    {
        Task SendMessage(string user, string message);
    }
}
