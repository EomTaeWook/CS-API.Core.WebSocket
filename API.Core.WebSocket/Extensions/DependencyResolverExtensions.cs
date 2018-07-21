using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace API.Core.WebSocket.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static T GetService<T>(this IDependencyResolver resolver)
        {
            return (T)resolver.GetService(typeof(T));
        }
        public static IEnumerable<T> GetServiceAll<T>(this IDependencyResolver resolver)
        {
            return resolver.GetServices(typeof(T)).Cast<T>();
        }
    }
}
