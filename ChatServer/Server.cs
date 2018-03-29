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

        ManualResetEvent allDone = new ManualResetEvent(false);

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

        /// <summary>
        /// The accept callback method is called when a new connection request is received on the socket.It is responsible for 
        /// getting the Socket instance that will handle the connection and handing that Socket off to the thread that will process the request
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Signal the main thread to continue.  
            allDone.Set();

            Console.WriteLine(handler.LocalEndPoint + " Connected");

            // Create the state object.  
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            // Read data from the client socket.  
            int read = handler.EndReceive(ar);

            // Data was read from the client socket.  
            if (read > 0)
            {
                state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, read));
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            else
            {
                if (state.Sb.Length > 1)
                {
                    // All the data has been read from the client;  
                    // display it on the console.  
                    string content = state.Sb.ToString();
                    Console.WriteLine("Read {0} bytes from socket.\n Data : {1}",
                       content.Length, content);
                }
                handler.Close();
            }
        }



    }
}
