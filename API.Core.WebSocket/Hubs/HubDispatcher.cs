﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using API.Core.WebSocket.Hubs.Lookup;
using API.Core.WebSocket.Extensions;
using System.Linq;
using API.Core.WebSocket.InternalStructure;
using API.Core.WebSocket.Hubs.Pipeline;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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
            Trace.WriteLine($"OnReceived : { data }");
            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(data);

            var setting = new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error
            };

            var hubDescriptor = _manager.GetHub(request.Value.Hub);
            var methodDescriptor = _manager.GetHubMethod(hubDescriptor.Name, request.Value.Method, request.Value.Args.ToArray());
            var hub = _activator.Create(hubDescriptor);
            hub.Clients = new HubConnectionContextBase(hubDescriptor.Name, Connection, _pipelineInvoker);
            methodDescriptor.Invoke(hub, request.Value.Args.Select((r, index) =>
            {
                return JsonSerializer.Deserialize(JToken.FromObject(r), methodDescriptor.Parameters[index].ParameterType);
            }).ToArray());
            hub.Dispose();

            return base.OnReceived(context, data);
        }
        internal static Task SendAsync(IHubPipelineInvokerContext context)
        {
            var hubMessage = context.Message;
            return context.Connection.Send(hubMessage);
        }
    }
}