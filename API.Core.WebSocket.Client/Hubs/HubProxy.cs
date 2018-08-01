using API.Core.WebSocket.Client.InternalStructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Client.Hubs
{
    public class HubProxy : IHubProxy
    {
        private readonly string _hubName;
        private readonly IConnection _connection;
        private readonly Dictionary<string, Subscription> _callbacks;
        private readonly JsonSerializerSettings _setting;

        public HubProxy(IConnection connection, string hubName)
        {
            _setting = new JsonSerializerSettings();
            _setting.TypeNameHandling = TypeNameHandling.All;
            _setting.MissingMemberHandling = MissingMemberHandling.Error;
            _connection = connection;
            _hubName = hubName;
            _callbacks = new Dictionary<string, Subscription>();
        }
        public Task Invoke(string method, params object[] args)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (args == null)
                throw new ArgumentNullException("args");

            var tokenArgs = new JToken[args.Length];
            for (int i = 0; i < tokenArgs.Length; i++)
            {
                tokenArgs[i] = args[i] != null ? JToken.FromObject(args[i]) : JValue.CreateNull();
            }
            var hubReqeust = new Message()
            {
                Value = new List<HubMessage>(){
                    new HubMessage()
                    {
                        Hub = _hubName,
                        Method = method,
                        Args = tokenArgs
                    }
                }
            };
            return _connection.Send(JsonConvert.SerializeObject(hubReqeust));
        }

        public Subscription Subscription(string methodName)
        {
            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");
            Subscription subscription;
            if (!_callbacks.TryGetValue(methodName, out subscription))
            {
                subscription = new Subscription();
                _callbacks.Add(methodName, subscription);
            }
            return subscription;
        }
        public void InvokeCallback(string methodName, IList<JToken> args)
        {
            Subscription subscription;
            if (_callbacks.TryGetValue(methodName, out subscription))
            {
                var paramInfo = subscription.OnCallback.Method.GetParameters();
                if (paramInfo.Length != args.Count)
                {
                    Trace.WriteLine("Invalid Number Of Arguments");
                    return;
                }
                subscription.OnCallback.DynamicInvoke(args.Select((r, index) =>
                    JsonConvert.DeserializeObject(r.ToString(), _setting)).ToArray());
            }
        }
    }
}
