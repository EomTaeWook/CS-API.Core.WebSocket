using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.WebSocket.Hubs;

namespace API.Core.WebSocket
{
    public abstract class Hub : IHub, IDisposable
    {
        private bool _disposed;
        protected Hub()
        {

        }
        public HostContext Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }
        protected virtual void Dispose(bool isDispose)
        {

        }
        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(true);
            _disposed = true;
        }
        public Task OnConnected()
        {
            return Task.CompletedTask;
        }

        public Task OnReconnected()
        {
            return Task.CompletedTask;
        }

        public Task OnDisconnected(bool stopCalled)
        {
            return Task.CompletedTask;
        }
    }
}
