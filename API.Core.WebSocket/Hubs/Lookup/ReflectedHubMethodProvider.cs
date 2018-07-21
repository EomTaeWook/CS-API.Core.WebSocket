using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs;
using System.Linq;
using System.Reflection;
using API.Core.WebSocket.Hubs.Extensions;
using API.Core.WebSocket.Hubs.Descriptor;

namespace API.Core.WebSocket.Default
{
    public class ReflectedHubMethodProvider : IHubMethodProvider
    {
        private ConcurrentDictionary<string, IDictionary<string, IEnumerable<MethodDescriptor>>> _methods;
        private ConcurrentDictionary<string, MethodDescriptor> _cacheMethods;
        private readonly Type[] _excludeTypes = new[] { typeof(Hub), typeof(object) };
        private readonly Type[] _excludeInterfaces = new[] { typeof(IHub), typeof(IDisposable)};

        public ReflectedHubMethodProvider()
        {
            _methods = new ConcurrentDictionary<string, IDictionary<string, IEnumerable<MethodDescriptor>>>();
            _cacheMethods = new ConcurrentDictionary<string, MethodDescriptor>();
        }
        private MethodDescriptor GetMethodDescriptor(string methodName, HubDescriptor hub, MethodInfo methodInfo)
        {
            return new MethodDescriptor()
            {
                Name = methodName,
                Hub = hub,
                Parameters = methodInfo.GetParameters(),
                Invoke = methodInfo.Invoke
            };
        }
        private IEnumerable<MethodInfo> GetInterfaceMethods(Type type, Type iface)
        {
            if (!iface.IsAssignableFrom(type))
            {
                return Enumerable.Empty<MethodInfo>();
            }
            return type.GetInterfaceMap(iface).TargetMethods;
        }


        public bool TryGetMethod(HubDescriptor hub, string method, out MethodDescriptor descriptor, object[] parameters)
        {
            var cacheKey = BuildHubExecutableMethodCacheKey(hub, method, parameters == null ? 0 : parameters.Length);
            if (!_cacheMethods.TryGetValue(cacheKey, out descriptor))
            {
                IEnumerable<MethodDescriptor> overloads;

                if (FetchMethodsFor(hub).TryGetValue(method, out overloads))
                {
                    var matches = overloads.Where(o => o.Parameters.Length == parameters.Length).ToList();
                    descriptor = matches.FirstOrDefault();
                }
                else
                {
                    descriptor = null;
                }
                if (descriptor != null)
                {
                    _cacheMethods.TryAdd(cacheKey, descriptor);
                }
            }

            return descriptor != null;
        }

        private IDictionary<string, IEnumerable<MethodDescriptor>> FetchMethodsFor(HubDescriptor hub)
        {
            var interfaceMethods = _excludeInterfaces.SelectMany(r => GetInterfaceMethods(hub.HubType, r));
            var humMethods = hub.HubType.GetMethods();
            var methods = humMethods.Except(interfaceMethods).Where(r =>
                !(_excludeTypes.Contains(r.GetBaseDefinition().DeclaringType) ||
                r.IsSpecialName));
            return methods.GroupBy(r => r.GetMethodName(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key,
                              g => g.Select(over => GetMethodDescriptor(g.Key, new HubDescriptor() { HubType = hub.GetType() }, over)),
                              StringComparer.OrdinalIgnoreCase);
        }
        public IEnumerable<MethodDescriptor> GetMethods(HubDescriptor hub)
        {
            return FetchMethodsFor(hub).SelectMany(r => r.Value).ToList();
        }
        private string BuildHubExecutableMethodCacheKey(HubDescriptor hub, string method, int paramsCount)
        {
            string countKeyPart;
            countKeyPart = paramsCount.ToString("d");
            return hub.Name + "::" + method.ToUpper() + "<" + countKeyPart + ">";
        }
    }
}
