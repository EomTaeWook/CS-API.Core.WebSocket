using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Client.InternalStructure
{
    [JsonObject]
    public class NegotiateResponse
    {
        public string Url { get; set; }
        public string ConnectionToken { get; set; }
        public string ConnectionID { get; set; }
    }
}
