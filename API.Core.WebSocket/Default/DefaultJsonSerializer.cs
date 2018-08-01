using API.Core.WebSocket.InternalStructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.Default
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private JsonSerializer _serializer;

        public DefaultJsonSerializer()
        {
            _serializer = new JsonSerializer();
            _serializer.NullValueHandling = NullValueHandling.Ignore;
            _serializer.MissingMemberHandling = MissingMemberHandling.Error;
        }
        public object Deserialize(JToken value, Type type)
        {
            return _serializer.Deserialize(new JTokenReader(value), type);
        }
    }
}
