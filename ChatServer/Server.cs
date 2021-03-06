﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.ServiceModel.Channels;
using System.Net;
using System.Net.NetworkInformation;
using System.Data.SQLite;
using ChatServer.Shared;

namespace ChatServer
{
    //https://docs.microsoft.com/en-us/dotnet/framework/network-programming/using-an-asynchronous-server-socket

    public class Server
    {
        private readonly int _port;
        private readonly IPAddress _ipAddress;
        private IPEndPoint _endPoint;
        
        private Socket _listener;
        
        private ManualResetEvent allDone = new ManualResetEvent(false);
        
        private List<Socket> clientList = new List<Socket>();
        
        private List<string> chatLog = new List<string>();

        private SQLDatabase _database;

        #region Constructor

        /// <summary>
        /// Opens the database and initialises the server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public Server(IPAddress ip, int port)
        {
            _database = new SQLDatabase();

            _port = port;
            _ipAddress = IPAddress.Any;

            _endPoint = new IPEndPoint(_ipAddress, _port);

            _listener = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region Server Communication

        /// <summary>
        /// Initialise the server and begin listening
        /// </summary>
        public void Start()
        {
            try
            {
                _database.Connect();

                Console.WriteLine("Connected to database");

                _listener.Bind(_endPoint);
                _listener.Listen(100);

                Console.WriteLine("Server has started");

                while (true)
                {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    _listener.BeginAccept(new AsyncCallback(AcceptCallback), _listener);


                    allDone.WaitOne();
                }
            }
            catch (Exception e)
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

            Console.WriteLine(handler.RemoteEndPoint + " Connected");

            // Create the state object.  
            StateObject state = new StateObject
            {
                WorkSocket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            clientList.Add(handler);
        }

        /// <summary>
        /// Run when the server receives a request from a client
        /// </summary>
        /// <param name="ar"></param>
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
            catch (SocketException e)
            {
                //Remove socket from client list
                clientList.Remove(state.WorkSocket);
                Console.WriteLine("ChatApp disconnected with error: " + e.ToString());
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

                    Task.Run(() =>
                    {
                        HandleData(content, state.WorkSocket);
                    });

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

        /// <summary>
        /// Sends data to a <see cref="Socket"/>
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        /// <summary>
        /// Run when the client <see cref="Socket"/> has accepted the data
        /// </summary>
        /// <param name="ar"></param>
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

        /// <summary>
        /// Sends specified data to all currently connected <see cref="Socket"/>
        /// </summary>
        /// <param name="data"></param>
        private void SendToAllConnctedSockets(string data)
        {
            foreach (var socket in clientList) // Repeat for each connected client (socket held in a dynamic array)
            {
                Send(socket, data); // call the above sendMessage function for sending message to a client
            }
        }

        #endregion

        #region Helper Functions
        
        /// <summary>
        /// Returns IPv4 address that has a default gateway
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddress()
        {
            foreach (var netI in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netI.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 && (netI.NetworkInterfaceType != NetworkInterfaceType.Ethernet || netI.OperationalStatus != OperationalStatus.Up))
                    continue;

                foreach (var uniIpAddrInfo in netI.GetIPProperties().UnicastAddresses.Where(x => netI.GetIPProperties().GatewayAddresses.Count > 0))
                {
                    if (uniIpAddrInfo.Address.AddressFamily == AddressFamily.InterNetwork && uniIpAddrInfo.AddressPreferredLifetime != uint.MaxValue)
                        return uniIpAddrInfo.Address.ToString();
                }
            }

            Console.WriteLine("You local IPv4 address couldn't be found...");
            return null;
        }

        /// <summary>
        /// Determines the type of request from the client and handles accordingly
        /// </summary>
        /// <param name="data"></param>
        private async Task HandleData(string data, Socket client)
        {
            // If the data is prefixed with -msg
            if(data.StartsWith(DataPrefix.Message.GetDescription()))
            {

            }

            // If the data is prefixed with -reguser
            if(data.StartsWith(DataPrefix.RegisterUser.GetDescription()))
            {
                var response = StringHelper.TrimAndSplitTcpResponse(DataPrefix.RegisterUser, data);

                string email = response[0];
                string firstName = response[1];
                string lastName = response[2];
                string password = response[3];

                Response dbResponse = new Response();

                try
                {
                    // Attempt to add user to database
                    dbResponse = await _database.AddUser(email, firstName, lastName, password);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }

                string responseToClient = BuildTcpResponse(DataPrefix.RegisterUser, dbResponse);

                Send(client, responseToClient);

                return;
            }

            //If the data is prefixed with -login
            if(data.StartsWith(DataPrefix.LoginUser.GetDescription()))
            {
                var response = StringHelper.TrimAndSplitTcpResponse(DataPrefix.LoginUser, data);

                string email = response[0];
                string password = response[1];

                Response dbResponse = new Response();

                try
                {
                    dbResponse = await _database.LogIn(email, password);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }

                string responseToClient = BuildTcpResponse(DataPrefix.LoginUser, dbResponse);

                Send(client, responseToClient);

                return;
            }

            return;
        }
        
        /// <summary>
        /// Builds a response to the client based on the results from the database
        /// </summary>
        /// <param name="prefix">The type of request that has been made</param>
        /// <param name="dbResponse">The response from the database</param>
        /// <returns></returns>
        private string BuildTcpResponse(DataPrefix prefix, Response dbResponse)
        {
            StringBuilder sb = new StringBuilder();

            if (dbResponse.Status == StatusCode.Success)
            {
                sb.Append(prefix.GetDescription());
                sb.Append(Constants.Delimiter);
                sb.Append(StatusCode.Success);
                sb.Append(Constants.Delimiter);
                sb.Append(Constants.EndOfFile);
            }
            else
            {
                sb.Append(prefix.GetDescription());
                sb.Append(Constants.Delimiter);
                sb.Append(StatusCode.Failure);
                sb.Append(Constants.Delimiter);
                sb.Append(dbResponse.Error);
                sb.Append(Constants.EndOfFile);
            }

            return sb.ToString();
        }


        #endregion
    }
}
