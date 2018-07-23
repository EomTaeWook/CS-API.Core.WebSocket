using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs
{
    public interface IHub
    {
        HostContext Context { get; }
        Task OnConnected();
        Task OnReconnected();
        Task OnDisconnected(bool stopCalled);
    }
}
