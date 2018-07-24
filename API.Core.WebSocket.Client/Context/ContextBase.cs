using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Core.WebSocket.Client.InternalStructure;

namespace API.Core.WebSocket.Client.Context
{
    public abstract class ContextBase : IContextClient
    {
        private bool _disposed;
        protected ContextBase()
        {
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

        public Task<NegotiateResponse> Negotiate(IConnection connection)
        {
            var _reqeust = WebRequest.Create(connection.Url + GetUrl(ConnectionType.Connect));
            NegotiateResponse negotiateResponse = null;
            using (HttpWebResponse resp = (HttpWebResponse)_reqeust.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;
                if (status == HttpStatusCode.OK)
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        GetNegotiationResponse(sr.ReadToEnd(), out negotiateResponse);
                    }
                }
            }
            if (negotiateResponse == null)
                throw new Exception("Connect Error");

            return Task.FromResult(negotiateResponse);
        }
        protected NegotiateResponse GetNegotiationResponse(string response, out NegotiateResponse negotiateResponse)
        {
            if (String.IsNullOrEmpty(response))
                return negotiateResponse = null;
            negotiateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<NegotiateResponse>(response);
            return negotiateResponse;
        }

        protected string GetUrl(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Connect:
                    return "/negotiation";
                case ConnectionType.ConnectQuery:
                    return "connectionToken";
            }
            return String.Empty;
        }

        public virtual Task Send(IConnection connection, string data)
        {
            return Task.CompletedTask;
        }

        public virtual Task Start(IConnection connection, CancellationToken disconnectToken)
        {
            if (connection == null)
                throw new ArgumentNullException("Connection");
            OnStart(connection, disconnectToken);
            return Task.CompletedTask;
        }
        protected abstract void OnStart(IConnection connection, CancellationToken disconnectToken);

        public Task Close(IConnection connection, CancellationToken disconnectToken)
        {
            OnClose(connection, disconnectToken);
            disconnectToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
        protected abstract void OnClose(IConnection connection, CancellationToken disconnectToken);
    }
}
