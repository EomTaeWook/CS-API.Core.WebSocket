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
        public Task ProcessReqeust(HostContext context)
        {
            var task = context.Request.HttpContext.WebSockets.AcceptWebSocketAsync();
            return ProcessReqeust(task.Result);
        }
        public override void OnMessage(byte[] message)
        {
            Received?.Invoke(Encoding.UTF8.GetString(message));
        }
        public override void OnMessage(string message)
        {
            Received?.Invoke(message);
        }
        public override Task Send(string message)
        {
            return base.Send(message);
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
