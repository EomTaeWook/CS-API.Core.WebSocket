using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    public class ConnectionProxy : ProxyBase
    {
        public ConnectionProxy(string hubName, IConnection connection) : base(hubName, connection)
        {
        }
    }
}
