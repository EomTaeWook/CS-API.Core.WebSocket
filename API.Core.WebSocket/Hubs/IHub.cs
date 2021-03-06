﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs
{
    public interface IHub : IDisposable
    {
        HostContext Context { get; set; }
        IHubConnectionContext<dynamic> Clients { get; set; }

        Task OnConnected();
        Task OnReconnected();
        Task OnDisconnected(bool stopCalled);
    }
}
