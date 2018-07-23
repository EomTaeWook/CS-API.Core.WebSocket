using API.Core.WebSocket.Client.Context;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client
{
    public interface IConnection : IDisposable
    {
        string ConnectionID { get; }
        string ConnectionToken { get; }
        string Url { get; }
        IContextClient Client { get; }
        Task Send(string data);
        void OnReceived(string data);
    }
}
