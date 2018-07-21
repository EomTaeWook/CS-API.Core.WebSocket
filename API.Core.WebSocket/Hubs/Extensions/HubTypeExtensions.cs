using System;
using System.Collections.Generic;
using System.Text;
using API.Core.WebSocket.Hubs;
using System.Linq;

namespace API.Core.WebSocket.Hubs.Extensions
{
    internal static class HubTypeExtensions
    {
        internal static string GetHubName(this Type type)
        {
            if (!typeof(IHub).IsAssignableFrom(type))
            {
                return null;
            }

            return GetHubAttributeName(type) ?? GetHubTypeName(type);
        }
        private static string GetHubAttributeName(this Type type)
        {
            if (!typeof(IHub).IsAssignableFrom(type))
            {
                return null;
            }
            var attributes = type.GetCustomAttributes(false).Where(r => r is HubNameAttribute).ToList();
            if (attributes.Any())
                return (attributes[0] as HubNameAttribute).HubName;
            return null;
        }
        private static string GetHubTypeName(this Type type)
        {
            if (!typeof(IHub).IsAssignableFrom(type))
            {
                return null;
            }
            return type.Name;
        }
    }
}
