using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs.Lookup;
using API.Core.WebSocket.Extensions;
using API.Core.WebSocket.Hubs;
using API.Core.WebSocket.Hubs.Descriptor;
using System.Linq;

namespace API.Core.WebSocket.Hubs.Lookup
{
    public class DefaultHubManager : IHubManager
    {
        private readonly IEnumerable<IHubProvider> _hubProviders;
        private readonly IEnumerable<IHubMethodProvider> _hubMethodProviders;
        public DefaultHubManager(IDependencyResolver resolver)
        {
            _hubProviders = resolver.GetServiceAll<IHubProvider>();
            _hubMethodProviders = resolver.GetServiceAll<IHubMethodProvider>();
        }

        public HubDescriptor GetHub(string hubName)
        {
            HubDescriptor descriptor = null;
            _hubProviders.FirstOrDefault(r => r.TryGetHub(hubName, out descriptor));
            return descriptor;
        }

        public MethodDescriptor GetHubMethod(string hubName, string method, object[] parameters)
        {
            HubDescriptor hub = GetHub(hubName);
            if (hub == null)
                return null;
            MethodDescriptor descriptor = null;
            if (_hubMethodProviders.FirstOrDefault(p => p.TryGetMethod(hub, method, out descriptor, parameters)) != null)
                return descriptor;
            return null;
        }

        public IEnumerable<HubDescriptor> GetHubs(string[] hubNames)
        {
            var hubs = _hubProviders.SelectMany(p => p.GetHubs());
            return hubs.Where(r => hubNames.Contains(r.Name));
        }
    }
}
