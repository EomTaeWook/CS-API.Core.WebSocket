using API.Core.WebSocket.Hubs.Pipeline;
using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    public class ConnectionProxy : HubProxy
    {
        public ConnectionProxy(string hubName, IConnection connection, IHubPipelineInvoker invoker) : base(hubName, connection, invoker)
        {
        }
    }
}
