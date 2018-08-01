using API.Core.WebSocket.Client.Hubs;
using API.Core.WebSocket.Client.InternalStructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client
{
    public class HubConnection : Connection
    {
        private readonly Dictionary<string, HubProxy> _hubs;
        public HubConnection() : base("")
        {

        }
        public HubConnection(string url) : base(url)
        {
            _hubs = new Dictionary<string, HubProxy>(StringComparer.OrdinalIgnoreCase);
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
        protected override void OnMessageReceived(JToken message)
        {
            var response = message.ToObject<Message>();
            foreach (var hubMessage in response.Value)
            {
                HubProxy hubProxy;
                if (_hubs.TryGetValue(hubMessage.Hub, out hubProxy))
                {
                    hubProxy.InvokeCallback(hubMessage.Method, hubMessage.Args);
                }
            }
            base.OnMessageReceived(message);
        }
    }
}
