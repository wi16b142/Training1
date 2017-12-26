using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Training1.Classes
{
    class Client
    {
        Socket clientSocket;
        byte[] buffer = new byte[512];
        Thread clientReceivingThread;
        Action<string> MessageInformer;

        public Client()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Loopback, 8055);
            clientSocket = client.Client;
            StartReceiving();
        }

        private void StartReceiving()
        {
            clientReceivingThread = new Thread(new ThreadStart(Receive));
            clientReceivingThread.IsBackground = true;
            clientReceivingThread.Start();
        }

        private void Receive()
        {
            string message = "";
            while (clientReceivingThread.IsAlive)
            {
                message = Encoding.UTF8.GetString(buffer, 0 , clientSocket.Receive(buffer));
                MessageInformer(message);
            }
            Close();
        }

        public void StopReceiving()
        {
            clientReceivingThread.Abort();
        }

        private void Close()
        {
            clientSocket.Close();
        }
    }
}
