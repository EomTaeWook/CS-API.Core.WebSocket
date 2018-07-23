using API.Core.WebSocket.Client.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client.Hubs
{
    public interface IHubProxy
    {
        Task Invoke(string method, params object[] args);
        Subscription Subscription(string methodName);
    }
}
