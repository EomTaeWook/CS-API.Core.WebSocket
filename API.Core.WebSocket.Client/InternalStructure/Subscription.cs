using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Client.InternalStructure
{
    public class Subscription
    {
        public MulticastDelegate OnCallback { get; set; }
    }
}
