using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs.Pipeline
{
    public interface IHubPipelineInvokerContext
    {
        Message Message { get; }
        IConnection Connection { get; }
    }
}
