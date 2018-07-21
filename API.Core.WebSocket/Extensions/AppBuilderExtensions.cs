using Microsoft.AspNetCore.Builder;
using API.Core.WebSocket.Default;
using API.Core.WebSocket.Middleware;

namespace API.Core.WebSocket.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void RunHubSocket(this IApplicationBuilder builder)
        {
            var resolver = new DefaultDependencyResolver();
            builder.UseWebSockets();
            builder.UseMiddleware<HubDispatcherMiddleware>(resolver);
        }
    }
}
