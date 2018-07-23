using API.Core.WebSocket.InternalStructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket
{
    public class HostContext : IHostContext
    {
        public virtual string ConnectionID { get; set; }
        public virtual HttpRequest Request { get; private set; }
        public virtual HttpResponse Response { get; private set; }

        protected HostContext() { }
        public HostContext(HttpRequest request)
        {
            Request = request;
            Response = request.HttpContext.Response;
        }
    }
}
