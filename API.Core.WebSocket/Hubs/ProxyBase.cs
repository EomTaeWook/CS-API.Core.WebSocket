using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs
{
    public abstract class ProxyBase : IClientProxy
    {
        protected string HubName { get; private set; }
        protected IConnection Connection { get; private set; }

        public ProxyBase(string hubName, IConnection connection)
        {
            HubName = hubName;
            Connection = connection;
        }

        public Task Invoke(string method, params object[] args)
        {
            var hubInvoke = GetInvocationData(method, args);
            var message = new Message();
            message.Value = new List<HubMessage>() { hubInvoke };
            return Connection.Send(message);

        }
        protected virtual HubMessage GetInvocationData(string method, object[] args)
        {
            return new HubMessage
            {
                Hub = HubName,
                Method = method,
                Args = args
            };
        }
    }
}
