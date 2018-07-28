using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Client.InternalStructure
{
    [JsonObject]
    public class Message
    {
        public string Key { get; set; }
        public IList<HubMessage> Value { get; set; }
        public string ConnectionID { get; set; }
        public string ConnectionToken { get; set; }
    }
}
