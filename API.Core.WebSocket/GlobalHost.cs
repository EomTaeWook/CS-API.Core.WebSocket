using API.Core.WebSocket.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket
{
    public static class GlobalHost
    {
        private static readonly Lazy<IDependencyResolver> _defaultResolver = new Lazy<IDependencyResolver>(() => new DefaultDependencyResolver());
        private static IDependencyResolver _resolver;
        public static IDependencyResolver DependencyResolver
        {
            get => _resolver ?? _defaultResolver.Value;
            set => _resolver = value;
        }
    }
}
