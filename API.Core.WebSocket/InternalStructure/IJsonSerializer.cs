using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    public interface IJsonSerializer
    {
        object Deserialize(JToken value, Type type);
    }
}
