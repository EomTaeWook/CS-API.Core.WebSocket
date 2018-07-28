using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubConnectionContext<T>
    {
        T All { get; }

        IHubProxy Current { get; }
    }
}
