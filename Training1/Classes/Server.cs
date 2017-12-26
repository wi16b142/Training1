using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Training1.Classes
{
    public class Server
    {
        private Socket serverSocket;
        List<ClientHandler> clients = new List<ClientHandler>();
        Thread acceptingThread;

        public Server()
        {
            //todo add params to constr
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8055));
            serverSocket.Listen(10);           
        }

        private void StartAccepting()
        {
            //accept clients in thread
            acceptingThread = new Thread(new ThreadStart(Accept));
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        private void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                try
                {
                    clients.Add(new ClientHandler(serverSocket.Accept()));
                }catch (Exception)
                {

                }
            }
        }

        public void StopAccepting()
        {
            serverSocket.Close();
            acceptingThread.Abort();

            foreach(var client in clients)
            {
                client.Close();
            }
            clients.Clear();
        }
    }
}
