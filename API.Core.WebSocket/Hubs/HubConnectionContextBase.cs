using API.Core.WebSocket.Hubs.Pipeline;
using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    public class HubConnectionContextBase : IHubConnectionContext<object>
    {
        protected string HubName { get; private set; }
        public dynamic All => throw new NotImplementedException();
        public IHubProxy Current { get; private set; }
        protected IConnection Connection { get; private set; }
        protected IHubPipelineInvoker Invoker { get; private set; }

        public HubConnectionContextBase(string hubName, IConnection connection, IHubPipelineInvoker invoker)
        {
            HubName = hubName;
            Connection = connection;
            Invoker = invoker;
            Current = new ConnectionProxy(hubName, connection, invoker);
        }
        protected HubConnectionContextBase()
        {
        }
    }
}
