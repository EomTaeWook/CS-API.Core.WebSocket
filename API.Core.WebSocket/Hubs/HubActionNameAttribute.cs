using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class HubActionNameAttribute : Attribute
    {
        public HubActionNameAttribute(string name)
        {
            ActionName = name;
        }
        public string ActionName { get; }
    }
}
