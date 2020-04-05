using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RawTcp.Server
{
    public class SocketReceiver
    {
        private readonly Socket _socket;
        private readonly SocketAsyncEventArgs _eventArgs = new SocketAsyncEventArgs();

        public SocketReceiver(Socket socket)
        {
            _socket = socket;

            _eventArgs.UserToken = "Karl";
            _eventArgs.Completed += (_, e) => { Console.WriteLine($""); };
        }


        public Task ReceiveAsync(Memory<byte> buffer)
        {
#if NETCOREAPP
            _eventArgs.SetBuffer(buffer);
#else
            var segment = buffer.GetArray();

            _eventArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);
#endif
            if (!_socket.ReceiveAsync(_eventArgs))
            {
                //_awaitable.Complete(_eventArgs.BytesTransferred, _eventArgs.SocketError);
            }

            return Task.CompletedTask;
        }
    }
}
