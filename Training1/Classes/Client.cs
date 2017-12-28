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
        private Socket clientSocket;
        private byte[] buffer = new byte[512];
        private Action<string> GuiUpdater;
        private Action AbortInfo;
        private Thread receivingThread;

        public Client(string ip, int port, Action<string> guiUpdater, Action abortInfo)
        {
            try
            {
                this.GuiUpdater = guiUpdater;
                this.AbortInfo = abortInfo;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ip), port);
                clientSocket = client.Client;
                StartReceiving();
            }catch (Exception)
            {
                //server not ready
            }
        }

        private void StartReceiving()
        {
            //Task.Factory.StartNew(Receive);
            receivingThread = new Thread(new ThreadStart(Receive));
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void Receive()
        {
            string update = "";
            while (receivingThread.IsAlive)
            {
                if (!update.Equals("@quit"))
                {
                    int length = clientSocket.Receive(buffer);
                    update = Encoding.UTF8.GetString(buffer, 0, length);
                    GuiUpdater(update);
                }
                else
                {
                    Close();
                    AbortInfo();
                }
            }          
        }

        public void Close()
        {
            clientSocket.Close();
            receivingThread.Abort();
        }
    }
}
