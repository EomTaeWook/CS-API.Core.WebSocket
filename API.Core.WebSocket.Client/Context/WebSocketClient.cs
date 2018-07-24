using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using API.Core.WebSocket.Client.InternalStructure;

namespace API.Core.WebSocket.Client.Context
{
    public class WebSocketClient : ContextBase
    {
        private ClientWebSocket _clientWebSocket;
        public WebSocketClient()
        {
            _clientWebSocket = new ClientWebSocket();
        }

        public override Task Send(IConnection connection, string data)
        {
            if (connection == null)
                throw new ArgumentNullException("Connection");
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
            return _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        protected override void OnClose(IConnection connection, CancellationToken disconnectToken)
        {
        }
        protected override void OnStart(IConnection connection, CancellationToken disconnectToken)
        {
            var builder = new UriBuilder(connection.Url);
            builder.Query = GetUrl(ConnectionType.ConnectQuery) + "=" + HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(connection.ConnectionToken));
            builder.Scheme = builder.Scheme.Equals("http") ? "ws" : "wss";

            _clientWebSocket.ConnectAsync(builder.Uri, disconnectToken).Wait();
        }
    }
}
