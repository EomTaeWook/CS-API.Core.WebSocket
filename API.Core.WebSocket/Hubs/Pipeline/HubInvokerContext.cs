using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.InternalStructure;

namespace API.Core.WebSocket.Hubs.Pipeline
{
    public class HubInvokerContext : IHubPipelineInvokerContext
    {
        public Message Message { get; set; }
        public IConnection Connection { get; set; }
        public HubInvokerContext()
        {

        }
        HubInvokerContext(IConnection connection, Message message)
        {
            Connection = connection;
            Message = message;
        }
    }
}
