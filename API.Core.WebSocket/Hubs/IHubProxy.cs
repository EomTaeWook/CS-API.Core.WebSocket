using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubProxy
    {
        Task Invoke(string method, params object[] args);
    }
}
