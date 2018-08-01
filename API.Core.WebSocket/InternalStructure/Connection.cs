using API.Core.WebSocket.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.WebSocket.InternalStructure
{
    public class Connection : IConnection, IContextConnection
    {
        private readonly string _connectionID;
        private Func<Message, Task> _sendCallback;
        private object _callbackState;
        public Connection(string connectionID)
        {
            _connectionID = connectionID;
        }
        public Task Send(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            message.ConnectionID = _connectionID;
            if (_sendCallback != null)
                return _sendCallback(message);
            return Task.CompletedTask;
        }
        public void Send(Func<Message, Task> callback, object state)
        {
            _sendCallback = callback ?? throw new ArgumentNullException("callback");
            _callbackState = state;
        }
    }
}
