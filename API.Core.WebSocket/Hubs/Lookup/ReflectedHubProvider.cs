using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using API.Core.WebSocket.Hubs;
using API.Core.WebSocket.Hubs.Descriptor;
using API.Core.WebSocket.Hubs.Extensions;

namespace API.Core.WebSocket.Default
{
    public class ReflectedHubProvider : IHubProvider
    {
        private readonly Lazy<IDictionary<string, HubDescriptor>> _hubs;

        public ReflectedHubProvider()
        {
            _hubs = new Lazy<IDictionary<string, HubDescriptor>>(BuildHubsCache);
        }
        private bool IsHubType(Type type)
        {
            try
            {
                return typeof(IHub).IsAssignableFrom(type) &&
                !type.IsAbstract &&
                !type.IsInterface &&
                (type.Attributes.HasFlag(TypeAttributes.Public) || type.Attributes.HasFlag(TypeAttributes.NestedPublic));
            }
            catch
            {
                return false;
            }
        }
        protected IDictionary<string, HubDescriptor> BuildHubsCache()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(r => r.GetTypes()).Where(IsHubType).ToList();
            var entries = new Dictionary<string, HubDescriptor>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < types.Count(); i++)
            {
                var name = types[i].GetHubName();
                entries.Add(name, new HubDescriptor() { Name = types[i].GetHubName(),
                                                        HubType = types[i] });
            }
            return entries;
        }
        public IList<HubDescriptor> GetHubs()
        {
            return _hubs.Value.Values.ToList();
        }

        public bool TryGetHub(string hubName, out HubDescriptor hubProvider)
        {
            return _hubs.Value.TryGetValue(hubName, out hubProvider);
        }
    }
}
