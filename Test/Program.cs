using API.Core.WebSocket.Client;
using API.Core.WebSocket.Client.Hubs;
using API.Core.WebSocket.Client.Hubs.Extensions;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HubConnection("http://localhost");
            var proxy = new HubProxy(client, "Chat");
            proxy.On<string>("test", r => Console.WriteLine(r));

            client.Start();
            proxy.Invoke("Send", 12345);
            Console.ReadKey();
        }
    }
}
