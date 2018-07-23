using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    public interface IHostContext
    {
        string ConnectionID { get; }
        HttpRequest Request { get; }
        HttpResponse Response { get; }
    }
}
