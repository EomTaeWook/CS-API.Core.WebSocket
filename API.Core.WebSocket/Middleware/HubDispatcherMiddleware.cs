using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.WebSocket.InternalStructure;

namespace API.Core.WebSocket.Middleware
{
    public class HubDispatcherMiddleware
    {
        private readonly IDependencyResolver _resolver;
        public HubDispatcherMiddleware(RequestDelegate next, IDependencyResolver resolver)
        {
            _resolver = resolver;
        }
        public Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var dispatcher = new HubDispatcher();
            dispatcher.Init(_resolver);
            return dispatcher.ProcessRequest(context);
        }
    }
}
