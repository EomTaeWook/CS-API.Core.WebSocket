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
    public class Connection : IConnection
    {
        protected bool _connected;
        private bool _dispose;
        private readonly object _sycn;
        public string ConnectionID { get; private set; }
        public string ConnectionToken { get; private set; }
        public string Url { get; private set; }
        public IContextClient Client { get; private set; }

        private CancellationTokenSource _disconnectCts;

        public event Action<string> Received;
        public event Action Closed;
        public event Action<Exception> OnError;

        protected Connection(string url)
        {
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
                Monitor.TryEnter(_sycn);
                _disconnectCts = new CancellationTokenSource();
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

                return Client.Start(this, CancellationToken.None);
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        protected virtual void OnSending(JToken data)
        {

        }
        protected virtual void OnClosed()
        {
            Closed?.Invoke();
        }

        void IConnection.OnReceived(JToken message)
        {
            OnMessageReceived(message);
        }
        protected virtual void OnMessageReceived(JToken message)
        {
            if (Received != null)
            {
                try
                {
                    Received(message.ToString());
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }
        void IConnection.Disconnect()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            try
            {
                if (Monitor.TryEnter(this))
                {
                    _disconnectCts.Cancel();
                    _disconnectCts.Dispose();
                    OnClosed();
                }
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }
}
