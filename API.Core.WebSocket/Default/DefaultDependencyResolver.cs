using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs;
using API.Core.WebSocket.Hubs.Lookup;
using API.Core.WebSocket.InternalStructure;
using System.Linq;

namespace API.Core.WebSocket.Default
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, IList<Func<object>>> _resolvers;
        private readonly ConcurrentBag<IDisposable> _created;

        private bool _disposed;

        public DefaultDependencyResolver()
        {
            _resolvers = new Dictionary<Type, IList<Func<object>>>();
            _created = new ConcurrentBag<IDisposable>();

            RegisterDefaultServices();

        }
        private void RegisterDefaultServices()
        {
            var hubs = new Lazy<ReflectedHubProvider>(() => new ReflectedHubProvider());
            Register(typeof(IHubProvider), () => hubs.Value);

            var hubMethod = new Lazy<ReflectedHubMethodProvider>(() => new ReflectedHubMethodProvider());
            Register(typeof(IHubMethodProvider), () => hubMethod.Value);

            var protectData = new Lazy<AesProtectedData>(() => new AesProtectedData());
            Register(typeof(IProtectedData), () => protectData.Value);

            var hubManager = new Lazy<DefaultHubManager>(() => new DefaultHubManager(this));
            Register(typeof(IHubManager), () => hubManager.Value);


            var hubActivator = new Lazy<DefaultHubActivator>(() => new DefaultHubActivator(this));
            Register(typeof(IHubActivator), () => hubActivator.Value);

        }
        private void Dispose(bool isDispose)
        {
            foreach (var obj in _created)
            {
                obj.Dispose();
            }
            _disposed = isDispose;
        }
        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(true);
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            IList<Func<object>> activators;
            if (_resolvers.TryGetValue(serviceType, out activators))
                return Created(activators[0]);
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            IList<Func<object>> activators;
            if (_resolvers.TryGetValue(serviceType, out activators))
            {
                if (activators.Count == 0)
                {
                    return null;
                }
                return activators.Select(Created).ToList();
            }
            return null;
        }

        public void Register(Type serviceType, Func<object> activator)
        {
            if (activator == null)
            {
                throw new ArgumentNullException("activators");
            }
            IList<Func<object>> activators;
            if (!_resolvers.TryGetValue(serviceType, out activators))
            {
                activators = new List<Func<object>>();
                _resolvers.Add(serviceType, activators);
            }
            activators.Add(activator);
        }
        private object Created(Func<object> creator)
        {
            object obj = creator();
            if (obj is IDisposable)
                _created.Add(obj as IDisposable);
            return obj;
        }
    }
}
