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

        object IHubConnectionContext<object>.Current => throw new NotImplementedException();

        public HubConnectionContextBase(string hubName, IConnection connection)
        {
            HubName = hubName;
            Connection = connection;
            Current = new ConnectionProxy(hubName, connection);
        }
        protected HubConnectionContextBase()
        {

        }
    }
}
