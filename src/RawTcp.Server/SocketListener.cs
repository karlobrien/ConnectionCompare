using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace RawTcp.Server
{
    public class SocketListener
    {
        private readonly Socket _listenSocket;
        private readonly Semaphore _maxConnections;

        //will need pool of socket async event args for accept operations
        //will need pool of socket async event args for sending and receiving
        private readonly SocketReceiver _socketReceiver;

        public SocketListener()
        {
            _listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8087));

            _maxConnections = new Semaphore(5, 5);

            //_listenSocket.Listen(120);
        }


        public async Task StartAsync()
        {
            while (true)
            {
                var sock = await _listenSocket.AcceptAsync();
                //hand off the socket to a processor.
            }
        }


        public void Start()
        {
            _listenSocket.Listen(120);
            int count = 0;

            while (true)
            {
                count++;
                _maxConnections.WaitOne();
                //We could pop this from a pool
                SocketAsyncEventArgs sea = new SocketAsyncEventArgs();
                sea.UserToken = "hello";
                sea.Completed += Sea_Completed;

                var isAsync = _listenSocket.AcceptAsync(sea);
                if (!isAsync)
                {
                    //Handle synchronous
                    Console.WriteLine("Synchronous Complete");
                    ProcessReceive(sea);
                }
                Console.WriteLine(count);
                                                                 }
        }

        private void Sea_Completed(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("Complete Fired Async");

            //grab a send/receive from the pool and start listening.
            //return to the pool
            ProcessReceive(e);
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            Console.WriteLine($"[{e.AcceptSocket.RemoteEndPoint}]: connected");
        }
    }
}
