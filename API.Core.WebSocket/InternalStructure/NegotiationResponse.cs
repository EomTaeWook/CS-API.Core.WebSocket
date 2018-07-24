using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    [JsonObject]
    public class NegotiationResponse
    {
        public string ConnectionId { get; set; }
        public string ConnectionToken { get; set; }
        public string Url { get; set; }
    }
}
