using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using API.Core.WebSocket.Context;
using API.Core.WebSocket.InternalStructure;
using API.Core.WebSocket.Extensions;

namespace API.Core.WebSocket
{
    public abstract class DurabilityConnection
    {
        public IConnection Connection { get; private set; }
        protected IProtectedData ProtectedData { get; private set; }
        private bool _initialized;
        protected virtual Task OnConnected(HostContext context)
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnReceived(HostContext context, string data)
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnDisconnected(HostContext context, bool stopCalled)
        {
            return Task.CompletedTask;
        }

        protected DurabilityConnection()
        {
        }
        public virtual void Init(IDependencyResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException("resolver");
            if (_initialized)
                return;

            ProtectedData = resolver.GetService<IProtectedData>();
            _initialized = true;
        }
        private bool IsNegotiationRequest(HttpRequest request)
        {
            return request.Path.ToString().EndsWith("/negotiation", StringComparison.OrdinalIgnoreCase);
        }
        public virtual Task ProcessRequest(HttpContext context)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("ConnectionNotInitialized");
            }
            var hostContext = new HostContext(context.Request);
            return ProcessRequest(hostContext);
        }
        private Task ProcessRequest(HostContext context)
        {

            if (IsNegotiationRequest(context.Request))
            {
                return ProcessNegotiationRequest(context);
            }

            string connectionToken = context.Request.Query["connectionToken"];
            if (String.IsNullOrEmpty(connectionToken))
            {
                return FailResponse(context.Response, "Bad Reqeust");
            }
            string connectionID;
            string message;
            int statusCode;
            TryGetConnectionID(context, connectionToken, out connectionID, out message, out statusCode);
            if (String.IsNullOrEmpty(connectionID))
            {
                return FailResponse(context.Response, message);
            }
            context.ConnectionID = connectionID;
            if (context.Request.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var stateObject = new DefaultWebSocketContext();
                stateObject.Received = data => { return OnReceived(context, data); };

                stateObject.Connected = () => { return OnConnected(context); };

                stateObject.Disconnected = clean => { return OnDisconnected(context, clean); };
                Connection = stateObject;
                return stateObject.ProcessReqeust(context);
            }
            return Task.CompletedTask;
        }

        private bool TryGetConnectionID(HostContext context, string connectionToken, out string connectionID, out string message, out int statusCode)
        {
            string unprotectedConnectionToken = null;
            connectionID = null;
            message = null;
            statusCode = 400;

            try
            {
                unprotectedConnectionToken = ProtectedData.Unprotect(connectionToken);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Failed to process connectionToken {0}: {1}", connectionToken, ex);
            }
            if (String.IsNullOrEmpty(unprotectedConnectionToken))
            {
                message = "ConnectionIdIncorrectFormat";
                return false;
            }

            var tokens = unprotectedConnectionToken.Split(':', 2);
            if (tokens.Length < 1)
            {
                message = "ConnectionIdIncorrectFormat";
                return false;
            }
            connectionID = tokens[0];
            return true;
        }

        private Task ProcessNegotiationRequest(HostContext context)
        {
            string connectionID = Guid.NewGuid().ToString("d");
            string connectionToken = connectionID + ':';

            context.Response.ContentType = "application/json";
            var payload = new NegotiationResponse()
            {
                Url = context.Request.PathBase.ToString().Replace("/negotiation", ""),
                ConnectionToken = ProtectedData.Protect(connectionToken),
                ConnectionId = connectionID
            };
#if DEBUG
            return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.Indented));
#else
            return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
#endif
        }
        private Task FailResponse(HttpResponse response, string message, int statusCode = 400)
        {
            response.StatusCode = statusCode;
            return response.WriteAsync(message);
        }
    }
}
