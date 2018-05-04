using ChatServer;
using ChatServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp.Core
{
    /// <summary>
    /// Allows connections to chat server
    /// </summary>
    public class ChatClient
    {
        #region Private Fields

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent _connectDone = new ManualResetEvent(false);
        private static ManualResetEvent _sendDone = new ManualResetEvent(false);
        private static ManualResetEvent _receiveDone = new ManualResetEvent(false);

        /// <summary>
        /// Signals completion of user registration workflow
        /// </summary>
        private static ManualResetEvent _userRegisteredDone = new ManualResetEvent(false);
        
        /// <summary>
        /// Signals completion of user login workflow
        /// </summary>
        private static ManualResetEvent _userLoginDone = new ManualResetEvent(false);

        //private static IPHostEntry _ipHostInfo;

        /// <summary>
        /// The IP address to connect to
        /// </summary>
        private static IPAddress _ipAddress;

        /// <summary>
        /// The port number for the remote device.  
        /// </summary>
        private const int _port = 8008;

        /// <summary>
        /// The remote endpoint to connect to
        /// </summary>
        private static IPEndPoint _remoteEP;

        /// <summary>
        ///  Socket used to connect to server
        /// </summary>
        private static Socket _client;

        /// <summary>
        /// The response that is returned from the server when registering a user
        /// </summary>
        private static Response _registerUserResponse;

        /// <summary>
        /// The response that is returned from the server when logging in a user
        /// </summary>
        private static Response _loginResponse;

        #endregion

        #region Public Properties


        public static bool Connected => _client != null ? true : false;

        #endregion

        #region Server Communication

        /// <summary>
        /// Connects to the chat server
        /// </summary>
        /// <param name="userName"></param>
        public static void Connect()
        {
            try
            {
                string ipv4Address = "10.0.0.158";//GetLocalIpAddress(); // Replace with external IP for remote connections

                //_ipHostInfo = Dns.GetHostEntry(ipv4Address);

                _ipAddress = IPAddress.Parse(ipv4Address); //_ipHostInfo.AddressList.First(x=>x. AddressFamily == AddressFamily.InterNetwork);
                _remoteEP = new IPEndPoint(_ipAddress, _port);

                // Create a TCP/IP socket.
                _client = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                _client.BeginConnect(_remoteEP, new AsyncCallback(ConnectCallback), _client);

                _connectDone.WaitOne();
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
        }

        /// <summary>
        /// Callback received from server when attempting to connect
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                _connectDone.Set();

                Receive(client);
            }
            catch (Exception e)
            {
                // Close the socket as we are not processing any further
                _client.Close();

                // Indicate that the connection attempt has finished and allows application to continue as normal
                _connectDone.Set();

                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Begin recieiving data from the specified socket.
        /// </summary>
        /// <param name="client">Socket to begin receiving data from.</param>
        private static void Receive(Socket client)
        {
            // Create the state object.  
            StateObject state = new StateObject
            {
                WorkSocket = client
            };

            client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// Run after the server has acknowledged our receive request
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.WorkSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                    content = state.Sb.ToString();

                    // Check for end-of-file tag. If it is not there, read   
                    // more data. 
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        HandleData(content);
                        
                        //Removes previous data received
                        state.DeleteData();
                    }

                    // Continue receiving  
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.GetType().ToString() + e.Message);
            }
        }

        /// <summary>
        /// Begins sending data to the server
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        private static void Send(Socket client, string data)
        {
            //Append end of file tag to data
            data = data + "<EOF>";

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        /// <summary>
        /// Run when server has acknowledged our send request
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                _sendDone.Set();

                //Begin receiving data again
                Receive(client);

            }
            catch (Exception e)
            {
                // Close the socket as we are not processing any further
                _client.Close();

                // Indicates that the send attempt has finished and allows application to continue as normal
                _sendDone.Set();

                throw new Exception(e.GetType().ToString() + e.Message);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event to indicate a user has been registered
        /// </summary>
        private static event EventHandler<ServerResponseEventArgs> ServerResponse;

        /// <summary>
        /// Raises UserRegistered event
        /// </summary>
        private static void OnServerResponse(ServerResponseEventArgs e)
        {
            ServerResponse?.Invoke(null, e);
        }

        /// <summary>
        /// Executed when the <see cref="UserRegistered"</see> event is fired/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ChatClient_ServerResponse(object sender, ServerResponseEventArgs e)
        {
            if (e.Type == DataPrefix.RegisterUser)
            {
                _registerUserResponse = new Response
                {
                    Status = e.Status,
                    Error = e.Error
                };

                _userRegisteredDone.Set();

            }

            if(e.Type == DataPrefix.LoginUser)
            {
                _loginResponse = new Response
                {
                    Status = e.Status,
                    Error = e.Error,
                };

                _userLoginDone.Set();
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Determines the type of data from the server and handles accordingly
        /// </summary>
        /// <param name="data"></param>
        private static void HandleData(string data)
        {
            DataPrefix type = DataPrefix.None;
            StatusCode status = StatusCode.None;
            string error = string.Empty;

            // If the data is prefixed with -msg
            if (data.StartsWith(DataPrefix.Message.GetDescription()))
            {
                //Rebuild message object
                //Message message = Message.BuildMessageFromTcpString(content);

                //Raise message received event
                // MessageReceivedEventArgs args = Message.BuildMessageReceivedEvent(message);
                //message.OnMessageReceived(args);
            }

            // If the data is prefixed with -reguser
            else if (data.StartsWith(DataPrefix.RegisterUser.GetDescription()))
            {
                // Trims and splits response
                string[] response = StringHelper.TrimAndSplitTcpResponse(DataPrefix.RegisterUser, data);

                type = DataPrefix.RegisterUser;
                status = (StatusCode)Enum.Parse(typeof(StatusCode), response[0]);
                error = response[1];
            }

            // If the data is prefixed with -login
            else if(data.StartsWith(DataPrefix.LoginUser.GetDescription()))
            {
                string[] response = StringHelper.TrimAndSplitTcpResponse(DataPrefix.LoginUser, data);

                type = DataPrefix.LoginUser;
                status = (StatusCode)Enum.Parse(typeof(StatusCode), response[0]);
                error = response[1];
            }

            if(type != DataPrefix.None)
            {
                ServerResponseEventArgs args = new ServerResponseEventArgs
                {
                    Type = type,
                    Status = status,
                    Error = error
                };

                // Raise server response event 
                OnServerResponse(args);
            }
        }

        /// <summary>
        /// Returns IPv4 address that has a default gateway
        /// </summary>
        /// <returns></returns>
        private static string GetLocalIpAddress()
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

        #region Send Message

        /// <summary>
        /// Processes and sends a <see cref="Message"/>
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(Message message)
        {
            // Converts a Message object to a string
            string messageString = message.BuildTcpString();

            // Send data to the server
            Send(_client, messageString);
            _sendDone.WaitOne();
        }

        #endregion

        #region Register User

        /// <summary>
        /// Sends a request to the server to register a user
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns></returns>
        public static async Task<Response> RegisterUser(RegisterUserToken user)
        {
            await Task.Run(() =>
            {
                // Set the ManualResetEvent to wait again in case the registration fails.
                _userRegisteredDone.Reset();

                ServerResponse += ChatClient_ServerResponse;

                string tcpString = user.BuildRegisterUserTcpString();

                // Send data to server and wait
                try
                {
                    Send(_client, tcpString);
                    _sendDone.WaitOne();
                }
                catch (ObjectDisposedException exc)
                {
                    Response response = new Response
                    {
                        Status = StatusCode.Failure,
                        Error = "Unable to connect to server"
                    };

                    _sendDone.Set();

                    _registerUserResponse = response;

                    return;
                }
                
                // Wait until server response to register user
                while (!_userRegisteredDone.WaitOne(0))
                {
                    Thread.SpinWait(1000);
                }
            });

            return _registerUserResponse;
        }

        #endregion

        #region Login

        /// <summary>
        /// Sends a request to the server to login the user
        /// </summary>
        /// <param name="loginToken">The token to login the user</param>
        /// <returns></returns>
        public static async Task<Response> Login(LoginToken loginToken)
        {
            await Task.Run(() =>
            {
                // Set the ManualResetEvent to wait again in case the registration fails.
                _userLoginDone.Reset();

                ServerResponse += ChatClient_ServerResponse;

                string tcpString = loginToken.BuildLoginTcpString();

                // Send data to server and wait
                try
                {
                    Send(_client, tcpString);
                    _sendDone.WaitOne();
                }
                catch (ObjectDisposedException exc)
                {
                    Response response = new Response
                    {
                        Status = StatusCode.Failure,
                        Error = "Unable to connect to server"
                    };

                    _sendDone.Set();

                    _loginResponse = response;

                    return;
                }

                // Wait until server response to register user
                while (!_userLoginDone.WaitOne(0))
                {
                    Thread.SpinWait(1000);
                }
            });

            return _loginResponse;
        }

        #endregion

        

        #endregion
    }
}
