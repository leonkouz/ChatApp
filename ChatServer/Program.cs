using System.Net;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(Server.GetLocalIpAddress());
            int port = 8008;

            Server server = new Server(ip, port);
            server.Start();
        }
    }
}
