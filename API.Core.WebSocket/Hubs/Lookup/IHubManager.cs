using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Hubs.Lookup
{
    public interface IHubManager
    {
        HubDescriptor GetHub(string hubName);
        IEnumerable<HubDescriptor> GetHubs(string[] hubNames);
        MethodDescriptor GetHubMethod(string hubName, string method, object[] parameters);
    }
}
