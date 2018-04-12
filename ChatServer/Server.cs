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

        List<Socket> clientList = new List<Socket>();

        List<string> chatLog = new List<string>();

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

            clientList.Add(handler);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int read = 0;
            try
            {
                // Read data from the client socket. 
                read = handler.EndReceive(ar);
            }
            catch(SocketException e)
            {
                //Remove socket from client list
                clientList.Remove(state.WorkSocket);
                Console.WriteLine("Client disconnected with error: " + e.ToString());
            }

            // Data was read from the client socket.  
            if (read > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, read));

                content = state.Sb.ToString();

                // Check for end-of-file tag. If it is not there, read   
                // more data. 
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);

                    //Removes previous data received
                    state.DeleteData();

                    SendDataToAllClients(content);

                    //Read more
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void SendDataToAllClients(string data)
        {
            foreach (var socket in clientList) // Repeat for each connected client (socket held in a dynamic array)
            {
                Send(socket, data); // call the above sendMessage function for sending message to a client
            }
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length,0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
