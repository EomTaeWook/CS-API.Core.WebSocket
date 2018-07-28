using API.Core.WebSocket.Client.Context;
using API.Core.WebSocket.Client.InternalStructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client
{
    public abstract class Connection : IConnection
    {
        protected bool _connected;
        private bool _dispose;
        private readonly object _sycn;
        public string ConnectionID { get; private set; }
        public string ConnectionToken { get; private set; }
        public string Url { get; private set; }
        public IContextClient Client { get; private set; }

        private CancellationTokenSource _disconnectCts;

        protected Connection(string url)
        {
            _disconnectCts = new CancellationTokenSource();
            _sycn = new object();
            Url = url;
        }
        protected virtual void Dispose(bool isDispose)
        {
        }
        public void Dispose()
        {
            if (_dispose)
                return;
            Dispose(true);
            _dispose = true;
        }
        public Task Send(string data)
        {
            return Client.Send(this, data);
        }
        public Task Start()
        {
            try
            {
                Monitor.Enter(_sycn);
                return Negotiate(new WebSocketClient());
            }
            finally
            {
                Monitor.Exit(_sycn);
            }
        }
        private Task Negotiate(IContextClient client)
        {
            Client = client ?? throw new ArgumentNullException("client");

            return Client.Negotiate(this).ContinueWith(r =>
            {
                ConnectionID = r.Result.ConnectionID;
                ConnectionToken = r.Result.ConnectionToken;

                return Client.Start(this, _disconnectCts.Token);
            });
        }
        public void OnReceived(string data)
        {   

        }
    }
}
