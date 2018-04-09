using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    class ChatClient
    {
        // The port number for the remote device.  
        private const int _port = 54003;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static IPHostEntry _ipHostInfo;
        private static IPAddress _ipAddress;
        private static IPEndPoint _remoteEP;

        private static Socket _client;

        private static string _userName;

        public static string UserName
        {
            get { return _userName; }
        }

        public static void Connect(string userName)
        {
            _userName = userName;

            _ipHostInfo = Dns.GetHostEntry("10.0.0.158");
            _ipAddress = IPAddress.Parse("10.0.0.158"); //_ipHostInfo.AddressList.First(x=>x. AddressFamily == AddressFamily. InterNetwork);
            _remoteEP = new IPEndPoint(_ipAddress, _port);

            // Create a TCP/IP socket.
            _client = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            _client.BeginConnect(_remoteEP, new AsyncCallback(ConnectCallback), _client);
            connectDone.WaitOne();
        }

        public static void SendMessage(Message message)
        {
            string messageString = message.BuildTcpString();

            // Send data to the server
            Send(_client, messageString);
            sendDone.WaitOne();
        }

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
                connectDone.Set();

                Receive(client);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());

                // Close the socket
                _client.Close();

                // Indicate that the connection attempt has finished
                connectDone.Set();
            }
                
        }

        /// <summary>
        /// Begin recieiving data from the specified socket.
        /// </summary>
        /// <param name="client">Socket to begin receiving data from.</param>
        private static void Receive(Socket client)
        {
            
                // Create the state object.  
                StateObject state = new StateObject();
                state.WorkSocket = client;

                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            
        }

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
                        //Rebuild message object
                        Message message = Message.BuildMessageFromTcpString(content);

                        //Raise message received event
                        MessageReceivedEventArgs args = Message.BuildMessageReceivedEvent(message);
                        message.OnMessageReceived(args);

                        //Removes previous data received
                        state.DeleteData();
                    }

                    // Continue receiving  
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
        }

        private static void Send(Socket client, string data)
        {
            //Append end of file tag to data
            data = data + "<EOF>";

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

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
                sendDone.Set();

                //Begin receiving data again
                Receive(client);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
