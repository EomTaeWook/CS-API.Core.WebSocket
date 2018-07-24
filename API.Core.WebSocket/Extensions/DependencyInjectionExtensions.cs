using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Default;
using API.Core.WebSocket.Hubs;

namespace API.Core.WebSocket.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWebSocketCore(this IServiceCollection services)
        {
            services.TryAddSingleton<DefaultDependencyResolver>();
            services.TryAddSingleton<HubDispatcher>();
            return services;
        }

    }
}
