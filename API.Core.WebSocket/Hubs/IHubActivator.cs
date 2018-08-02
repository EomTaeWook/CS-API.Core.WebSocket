using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubActivator
    {
        Hub Create(HubDescriptor hubDescriptor);
    }
}
