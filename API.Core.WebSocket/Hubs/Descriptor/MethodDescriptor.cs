using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace API.Core.WebSocket.Hubs.Descriptor
{
    public class MethodDescriptor : Descriptor
    {
        public virtual ParameterInfo[] Parameters { get; set; }
        public virtual HubDescriptor Hub { get; set; }
        public virtual Func<object, object[], object> Invoke { get; set; }
    }
}
