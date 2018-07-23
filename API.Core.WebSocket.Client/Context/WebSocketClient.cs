using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        protected override void OnClose(IConnection connection, CancellationToken disconnectToken)
        {
            
        }

        protected override void OnStart(IConnection connection, CancellationToken disconnectToken)
        {
            var builder = new UriBuilder();
            builder.Host = connection.Url;
            Uri url = new Uri(connection.Url + GetUrl(ConnectionType.WebSocketConnect));
            _clientWebSocket.ConnectAsync(url, disconnectToken);
        }
    }
}
