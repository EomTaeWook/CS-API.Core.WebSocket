using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using API.Core.WebSocket.Hubs;
using API.Core.WebSocket.Hubs.Lookup;
using API.Core.WebSocket.Extensions;
using System.Linq;
using Newtonsoft.Json;

namespace API.Core.WebSocket.InternalStructure
{
    public class HubDispatcher : DurabilityConnection
    {
        private IHubManager _manager;
        private IHubActivator _activator;
        private readonly JsonSerializerSettings _setting;
        public HubDispatcher()
        {
            _setting = new JsonSerializerSettings();
            _setting.TypeNameHandling = TypeNameHandling.All;
            _setting.MissingMemberHandling = MissingMemberHandling.Error;
        }
        public override void Init(IDependencyResolver resolver)
        {
            _manager = resolver.GetService<IHubManager>();
            _activator = resolver.GetService<IHubActivator>();
            base.Init(resolver);
        }
        public override Task ProcessRequest(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

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
            var request = JsonConvert.DeserializeObject<HubRequest>(data);
            foreach (var req in request.Value)
            {
                var hubDescriptor = _manager.GetHub(req.Hub);
                var methodDescriptor = _manager.GetHubMethod(hubDescriptor.Name, req.Method, req.Args.ToArray());
                var hub = _activator.Create(hubDescriptor);

                methodDescriptor.Invoke(hub, req.Args.Select((r, index) =>
                    JsonConvert.DeserializeObject(r[index].ToString(), methodDescriptor.Parameters[index].ParameterType, _setting)).ToArray());
            }
            return base.OnReceived(context, data);
        }
    }
}