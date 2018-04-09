using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("10.0.0.158");
            int port = 54003;

            Server server = new Server(ip, port);
            server.Start();
        }
    }
}
