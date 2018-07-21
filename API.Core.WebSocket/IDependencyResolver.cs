using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket
{
    public interface IDependencyResolver : IDisposable
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
        void Register(Type serviceType, Func<object> activator);
    }
}
