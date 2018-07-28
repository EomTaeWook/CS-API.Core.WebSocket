using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Context
{
    public interface IContextConnection
    {
        void Send(Func<Message, Task> callback, object state);
    }
}
