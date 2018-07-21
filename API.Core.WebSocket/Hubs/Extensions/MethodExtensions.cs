using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace API.Core.WebSocket.Hubs.Extensions
{
    public static class MethodExtensions
    {
        internal static string GetMethodName(this MethodInfo method)
        {
            return GetMethodAttributeName(method) ?? method.Name;
        }
        private static string GetMethodAttributeName(MethodInfo method)
        {
            return method.GetCustomAttribute<HubActionNameAttribute>().ActionName;
        }
    }
}
