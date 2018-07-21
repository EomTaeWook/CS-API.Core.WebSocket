using System;
using System.Collections.Generic;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubProvider
    {
        IList<HubDescriptor> GetHubs();
        bool TryGetHub(string hubName, out HubDescriptor hubProvider);
    }
}
