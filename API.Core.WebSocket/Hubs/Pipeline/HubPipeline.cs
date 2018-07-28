using System;
using System.Threading.Tasks;

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
            if (Invoke != null)
                return Invoke(context);

            return Task.CompletedTask;
        }
    }
}
