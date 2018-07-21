using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    [JsonObject]
    public class HubDemand
    {
        [JsonProperty("H")]
        public string Hub { get; set; }
        [JsonProperty("M")]
        public string Method { get; set; }
        [JsonProperty("A")]
        public List<JToken> Args { get; set; }
    }
}
