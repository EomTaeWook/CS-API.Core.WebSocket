using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Context
{
    public class DefaultWebSocketContext : WebSocketContext
    {
        public DefaultWebSocketContext()
        {
        }
        public async Task ProcessReqeustAsync(HostContext context, IConnection connection)
        {
            var socket = await context.Request.HttpContext.WebSockets.AcceptWebSocketAsync();
            await ProcessReqeust(socket, connection as IContextConnection);
        }
        public override void OnMessage(byte[] message)
        {
            Received?.Invoke(Encoding.UTF8.GetString(message));
        }
        public override void OnMessage(string message)
        {
            Received?.Invoke(message);
        }

        public override Task OnSend(Message message)
        {
            return Send(message);
        }

        public override void OnError()
        {
            base.OnError();
        }
        public override void OnOpen()
        {
            base.OnOpen();
        }
        public override void OnClosed()
        {
            base.OnClosed();
        }
    }
}
