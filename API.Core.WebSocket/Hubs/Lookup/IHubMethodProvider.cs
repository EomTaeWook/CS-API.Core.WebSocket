using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubMethodProvider
    {
        IEnumerable<MethodDescriptor> GetMethods(HubDescriptor hub);
        bool TryGetMethod(HubDescriptor hub, string method, out MethodDescriptor invoke, object[] parmameter);
    }
}
