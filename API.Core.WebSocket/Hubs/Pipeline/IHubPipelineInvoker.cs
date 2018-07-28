using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs.Pipeline
{
    public interface IHubPipelineInvoker
    {
        Task Send(IHubPipelineInvokerContext context);
    }
}
