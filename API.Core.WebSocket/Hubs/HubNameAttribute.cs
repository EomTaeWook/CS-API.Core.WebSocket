using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HubNameAttribute : Attribute
    {
        public HubNameAttribute(string name)
        {
            HubName = name;
        }
        public string HubName { get; }
    }
}
