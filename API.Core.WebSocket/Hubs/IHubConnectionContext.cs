using API.Core.WebSocket.InternalStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    public interface IHubConnectionContext
    {
        IList<IHostContext> All { get; }
        IHostContext AllExcept(params string[] excludeConnectionIds);

        IHostContext Client(string connectionID);
        IList<IHostContext> Clients(IList<string> connectionIDs);

        IHostContext User(string userID);
        IList<IHostContext> Users(IList<string> userIDs);
    }
}
