using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Client.InternalStructure
{
    [JsonObject]
    public class HubMessage
    {
        [JsonProperty("H")]
        public string Hub { get; set; }
        [JsonProperty("M")]
        public string Method { get; set; }
        [JsonProperty("A")]
        public IList<JToken> Args { get; set; }
    }
}
