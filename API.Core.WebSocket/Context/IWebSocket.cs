using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Context
{
    interface IWebSocket
    {
        Action<string> OnMessage { get; set; }
        Action OnClose { get; set; }
        Action<Exception> OnError { get; set; }
        Task Send(ArraySegment<byte> message);
    }
}
