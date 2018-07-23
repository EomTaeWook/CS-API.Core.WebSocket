using API.Core.WebSocket.Client.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client.Context
{
    public interface IContextClient : IDisposable
    {
        Task<NegotiateResponse> Negotiate(IConnection connection);
        Task Send(IConnection connection, string data);
        Task Start(IConnection connection, CancellationToken disconnectToken);
        Task Close(IConnection connection, CancellationToken disconnectToken);
    }
}
