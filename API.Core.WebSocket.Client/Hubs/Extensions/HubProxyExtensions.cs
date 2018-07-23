using API.Core.WebSocket.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Client.Hubs.Extensions
{
    public static class HubProxyExtensions
    {
        public static void On(this IHubProxy proxy, string methodName, Action onAction)
        {
            if (proxy == null)
                throw new ArgumentNullException("proxy");
            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");

            var subscription = proxy.Subscription(methodName);
            subscription.OnCallback = onAction ?? throw new ArgumentNullException("onAction");
        }

        public static void On<T>(this IHubProxy proxy, string methodName, Action<T> onAction)
        {
            if(proxy == null)
                throw new ArgumentNullException("proxy");
            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");

            var subscription = proxy.Subscription(methodName);
            subscription.OnCallback = onAction ?? throw new ArgumentNullException("onAction");
        }

        public static void On<T1, T2>(this IHubProxy proxy, string methodName, Action<T1, T2> onAction)
        {
            if (proxy == null)
                throw new ArgumentNullException("proxy");
            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");

            var subscription = proxy.Subscription(methodName);
            subscription.OnCallback = onAction ?? throw new ArgumentNullException("onAction");

        }
        public static void On<T1, T2, T3>(this IHubProxy proxy, string methodName, Action<T1, T2, T3> onAction)
        {
            if (proxy == null)
                throw new ArgumentNullException("proxy");
            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");

            var subscription = proxy.Subscription(methodName);
            subscription.OnCallback = onAction ?? throw new ArgumentNullException("onAction");
        }
    }
}
