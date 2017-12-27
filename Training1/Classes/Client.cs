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
        Action<string> GuiUpdater;

        public Client(string ip, int port, Action<string> guiUpdater)
        {
            this.GuiUpdater = guiUpdater;
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ip), port);
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
            string update = "";
            while (clientReceivingThread.IsAlive)
            {
                update = Encoding.UTF8.GetString(buffer, 0 , clientSocket.Receive(buffer));
                GuiUpdater(update);
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
