using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.ServiceModel.Channels;
using System.Net;

namespace ChatServer
{
    //https://docs.microsoft.com/en-us/dotnet/framework/network-programming/using-an-asynchronous-server-socket


    public class Server
    {
        readonly int _port;
        readonly IPAddress _ipAddress;
        IPEndPoint _endPoint;

        Socket _listener;

        ManualResetEvent allDone;

        /// <summary>
        /// Initialises the server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public Server(IPAddress ip, int port)
        {
            _port = port;
            _ipAddress = ip;

            _endPoint = new IPEndPoint(_ipAddress, _port);

            _listener = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            try
            {
                _listener.Bind(_endPoint);
                _listener.Listen(100);

                Console.WriteLine("Server has started");

                while(true)
                {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    _listener.BeginAccept(new AsyncCallback(AcceptCallback), _listener);

                    allDone.WaitOne();
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            //The accept callback method is called when a new connection request is received on the socket.It is responsible for 
            //getting the Socket instance that will handle the connection and handing that Socket off to the thread that will process the request

            // Add the callback code here.  
        }


    }
}
