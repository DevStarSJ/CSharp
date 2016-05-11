using Microsoft.AspNet.Http;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networks
{
    public class WebSocketServer
    {
        public int BufferSize { get; set; } = 4096;

        private ConcurrentBag<WebSocket> ClientSockets = new ConcurrentBag<WebSocket>();

        public async Task Start(HttpContext http, Func<Task> next)
        {
            if (http.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await http.WebSockets.AcceptWebSocketAsync())
                {
                    if (webSocket != null && webSocket.State == WebSocketState.Open)
                    {
                        ManageEstablishedConnection(webSocket);

                        while (webSocket.State == WebSocketState.Open)
                        {
                            var token = CancellationToken.None;
                            var buffer = new ArraySegment<byte>(new byte[BufferSize]);

                            var received = await webSocket.ReceiveAsync(buffer, token);

                            switch (received.MessageType)
                            {
                                case WebSocketMessageType.Text:
                                    var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                                    await HandleRequestText(webSocket, buffer, token);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                await next();
            }
        }

        public void ManageEstablishedConnection(WebSocket webSocket)
        {
            ClientSockets.Add(webSocket);
        }

        public async Task HandleRequestText(WebSocket webSocket, ArraySegment<byte> message, CancellationToken token)
        {
            var request = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);

            foreach (var socket in ClientSockets)
            {
                await socket.SendAsync(message, WebSocketMessageType.Text, true, token);
            }
        }
    }
}
