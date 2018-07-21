using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Hubs.Descriptor
{
    public class HubDescriptor : Descriptor
    {
        public virtual Type HubType { get; set; }
    }
}
