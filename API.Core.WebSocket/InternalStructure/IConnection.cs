using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.InternalStructure
{
    public interface IConnection
    {
        Task Send(Message message);
    }
}
