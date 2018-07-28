using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.WebSocket.InternalStructure;

namespace API.Core.WebSocket.Hubs.Pipeline
{
    public class HubPipeline : IHubPipelineInvoker
    {
        public Func<IHubPipelineInvokerContext, Task> Invoke;
        public HubPipeline()
        {
            Invoke = HubDispatcher.SendAsync;
        }
        public Task Send(IHubPipelineInvokerContext context)
        {
            if(Invoke != null)
                return Invoke(context);

            return Task.CompletedTask;
        }
    }
}
