using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RawTcp.PreallocServer
{
    public class SocketSetup
    {
        private readonly Socket _listenSocket;
        private readonly Semaphore _maxConnections;

        //will need pool of socket async event args for accept operations
        //will need pool of socket async event args for sending and receiving

        public SocketSetup()
        {
            _listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8087));

            //make this configurable
            _maxConnections = new Semaphore(5, 5);

            _listenSocket.Listen(129);
        }

        public void Start()
        {
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
                Process(sea);
            }
        }

        private void Sea_Completed(object sender, SocketAsyncEventArgs e)
        {
            //We have 
        }

        private void Process(SocketAsyncEventArgs sea)
        {

        }
    }
}
