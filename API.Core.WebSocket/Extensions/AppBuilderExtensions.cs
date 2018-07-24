using Microsoft.AspNetCore.Builder;
using API.Core.WebSocket.Default;
using API.Core.WebSocket.Middleware;

namespace API.Core.WebSocket.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void RunHubSocket(this IApplicationBuilder builder)
        {
            builder.UseWebSockets();
            var resolver = GlobalHost.DependencyResolver ?? new DefaultDependencyResolver();
            builder.UseMiddleware<HubDispatcherMiddleware>(resolver);
        }
    }
}
