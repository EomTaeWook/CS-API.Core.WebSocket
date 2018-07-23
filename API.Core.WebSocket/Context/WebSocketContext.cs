﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Core.WebSocket.Context
{
    public abstract class WebSocketContext : IBaseContext, IDisposable
    {
        private readonly TimeSpan _closeTimeout = TimeSpan.FromMilliseconds(250);
        private System.Net.WebSockets.WebSocket _webSocket;
        private CancellationToken _disconnectToken;
        private bool _disposed;

        public virtual void OnOpen() { }
        public virtual void OnMessage(string message) { throw new NotImplementedException(); }
        public virtual void OnMessage(byte[] message) { throw new NotImplementedException(); }
        public virtual void OnError() { }
        public virtual void OnClosed() { }

        public Func<string, Task> Received { get; set; }
        public Func<Task> Connected { get; set; }
        public Func<Task> Reconnected { get; set; }
        public string ConnectionID { get; set; }
        public Func<bool, Task> Disconnected { get; set; }

        public Task ProcessReqeust(System.Net.WebSockets.WebSocket webSocket)
        {
            _webSocket = webSocket;
            _disconnectToken = new CancellationToken();
            OnOpen();
            return ReceiveAsync(_webSocket, _disconnectToken);
        }
        public virtual Task Send(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            return SendAsync(message);
        }
        private Task SendAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            return SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text);
        }

        private async Task ReceiveAsync(System.Net.WebSockets.WebSocket webSocket, CancellationToken disconnectToken)
        {
            bool closedReceived = false;
            var buffer = new byte[4096];
            var arraySegment = new ArraySegment<byte>(buffer);
            try
            {
                while (!disconnectToken.IsCancellationRequested && !closedReceived)
                {
                    var result = await webSocket.ReceiveAsync(arraySegment, disconnectToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        closedReceived = true;
                        await Task.WhenAny(CloseAsync(), Task.Delay(_closeTimeout));
                    }
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        OnMessage(Encoding.UTF8.GetString(arraySegment.Array));
                    }
                    else if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        OnMessage(arraySegment.Array);
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.Write("Error Receiving : " + ex.Message);
            }
            try
            {
                await CloseAsync();
            }
            finally
            {
                OnClosed();
            }
        }
        private async Task SendAsync(ArraySegment<byte> message, WebSocketMessageType messageType, bool endOfMessage = true)
        {
            if (_webSocket.State != WebSocketState.Open)
                return;
            try
            {
                await _webSocket.SendAsync(message, messageType, endOfMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Sending : " + ex.Message);
            }
        }
        public virtual Task CloseAsync()
        {
            if (_webSocket.State == WebSocketState.Closed ||
                _webSocket.State == WebSocketState.Aborted ||
                _webSocket.State == WebSocketState.CloseSent)
            {
                return Task.CompletedTask;
            }

            try
            {
                _disconnectToken.ThrowIfCancellationRequested();
                return _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Closing : " + ex.Message);
            }
            return Task.CompletedTask;
        }
        protected virtual void Dispose(bool isDispose)
        {

        }
        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(true);
            _disposed = true;
        }
    }
}