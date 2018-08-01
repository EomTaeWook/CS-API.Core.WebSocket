using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private List<byte> _reciveBuffer;
        private byte[] _buffer;
        private IConnection _connection;
        private CancellationToken _disconnectToken;
        private bool _closedReceived;

        public WebSocketClient()
        {
            _clientWebSocket = new ClientWebSocket();
            _reciveBuffer = new List<byte>();
            _buffer = new byte[4096];
        }
        public override Task Send(IConnection connection, string data)
        {
            if (connection == null)
                throw new ArgumentNullException("Connection");
            if (_clientWebSocket.State != WebSocketState.Open)
                throw new Exception("WebSocketState Not Open");

            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
            return _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        protected override void OnClose()
        {
            _connection.Disconnect();
        }

        protected override async void OnStart(IConnection connection, CancellationToken disconnectToken)
        {
            _connection = connection;
            _disconnectToken = disconnectToken;

            var builder = new UriBuilder(connection.Url);
            builder.Query = GetUrl(ConnectionType.ConnectQuery) + "=" + HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(connection.ConnectionToken));
            builder.Scheme = builder.Scheme.Equals("http") ? "ws" : "wss";

            await _clientWebSocket.ConnectAsync(builder.Uri, disconnectToken);
            await ProcessReqeustAsync();
        }

        protected override void OnStartFailed()
        {
            throw new NotImplementedException();
        }
        private async Task ProcessReqeustAsync()
        {
            while (!_disconnectToken.IsCancellationRequested && !_closedReceived)
            {
                await ReceiveAsync();
            }
            try
            {
                if (_clientWebSocket.State != WebSocketState.Closed && _clientWebSocket.State != WebSocketState.Aborted)
                    await CloseAsync();
            }
            finally
            {
                OnClose();
            }
        }
        private async Task ReceiveAsync()
        {
            var arraySegment = new ArraySegment<byte>(_buffer);
            WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(arraySegment, _disconnectToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                _disconnectToken.ThrowIfCancellationRequested();
                _closedReceived = true;
            }
            _reciveBuffer.InsertRange(_reciveBuffer.Count, _buffer.Take(result.Count).ToArray());
            if (result.EndOfMessage)
            {
                base.OnResponse(_connection, Encoding.UTF8.GetString(_reciveBuffer.ToArray()));
                _reciveBuffer.Clear();
            }

        }
        private Task CloseAsync()
        {
            if (_clientWebSocket.State == WebSocketState.Closed ||
                _clientWebSocket.State == WebSocketState.Aborted ||
                _clientWebSocket.State == WebSocketState.CloseSent)
            {
                return Task.CompletedTask;
            }

            try
            {
                _disconnectToken.ThrowIfCancellationRequested();
                return _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Closing : " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
