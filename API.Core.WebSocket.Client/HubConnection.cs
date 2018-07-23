using API.Core.WebSocket.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client
{
    public class HubConnection : Connection
    {
        private readonly Dictionary<string, HubProxy> _hubs = new Dictionary<string, HubProxy>(StringComparer.OrdinalIgnoreCase);
        public HubConnection() : base("")
        {

        }
        public HubConnection(string url) : base(url)
        {
        }
        public IHubProxy CreateHubProxy(string hubName)
        {
            HubProxy hubProxy;
            if (!_hubs.TryGetValue(hubName, out hubProxy))
            {
                hubProxy = new HubProxy(this, hubName);
                _hubs[hubName] = hubProxy;
            }
            return hubProxy;
        }
    }
}
