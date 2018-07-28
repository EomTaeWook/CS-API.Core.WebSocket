using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using API.Core.WebSocket.Hubs.Lookup;
using API.Core.WebSocket.Extensions;
using System.Linq;
using API.Core.WebSocket.InternalStructure;
using API.Core.WebSocket.Hubs.Pipeline;

namespace API.Core.WebSocket.Hubs
{
    public class HubDispatcher : DurabilityConnection
    {
        private IHubManager _manager;
        private IHubActivator _activator;
        private IHubPipelineInvoker _pipelineInvoker;
        public HubDispatcher()
        {
        }
        public override void Init(IDependencyResolver resolver)
        {
            _manager = resolver.GetService<IHubManager>();
            _activator = resolver.GetService<IHubActivator>();
            _pipelineInvoker = resolver.GetService<IHubPipelineInvoker>();
            base.Init(resolver);
        }
        public override Task ProcessRequest(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return base.ProcessRequest(context);
        }
        protected override Task OnConnected(HostContext context)
        {
            return base.OnConnected(context);
        }
        protected override Task OnDisconnected(HostContext context, bool stopCalled)
        {
            return base.OnDisconnected(context, stopCalled);
        }
        protected override Task OnReceived(HostContext context, string data)
        {
            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(data);

            var setting = new Newtonsoft.Json.JsonSerializerSettings();
            setting.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
            setting.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;

            foreach (var req in request.Value)
            {
                var hubDescriptor = _manager.GetHub(req.Hub);
                var methodDescriptor = _manager.GetHubMethod(hubDescriptor.Name, req.Method, req.Args.ToArray());
                var hub = _activator.Create(hubDescriptor);
                hub.Clients = new HubConnectionContextBase(hubDescriptor.Name, Connection, _pipelineInvoker);
                methodDescriptor.Invoke(hub,
                    req.Args.Select((r, index) => Newtonsoft.Json.JsonConvert.DeserializeObject(r.ToString(), methodDescriptor.Parameters[index].ParameterType, setting)).ToArray());
            }
            return base.OnReceived(context, data);
        }
        internal static Task SendAsync(IHubPipelineInvokerContext context)
        {
            var hubMessage = context.Message;
            return context.Connection.Send(hubMessage);
        }
    }
}