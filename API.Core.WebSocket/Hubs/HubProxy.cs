using API.Core.WebSocket.Hubs.Pipeline;
using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs
{
    public abstract class HubProxy : IHubProxy
    {
        protected string HubName { get; private set; }
        protected IConnection Connection { get; private set; }
        protected IHubPipelineInvoker Invoker { get; private set; }

        public HubProxy(string hubName, IConnection connection, IHubPipelineInvoker invoker)
        {
            HubName = hubName;
            Connection = connection;
            Invoker = invoker;
        }

        public Task Invoke(string method, params object[] args)
        {
            var hubInvoke = GetInvocationData(method, args);
            var message = new Message();
            message.Value = new List<HubMessage>() { hubInvoke };
            var InvokerContext = new HubInvokerContext()
            {
                Connection = Connection,
                Message = message
            };
            return Invoker.Send(InvokerContext);

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
