using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Training1.Classes
{
    public class Server
    {
        private Socket serverSocket, clientSocket;
        public Server()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8055));
            serverSocket.Listen(10);
            clientSocket = serverSocket.Accept();
        }
    }
}
