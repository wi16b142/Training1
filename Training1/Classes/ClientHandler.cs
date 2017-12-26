using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training1.Classes
{
    class ClientHandler
    {
        private Socket clientSocket;

        public Socket ClientSocket
        {
            get { return clientSocket; }
            set { clientSocket = value; }
        }

        public ClientHandler(Socket socket)
        {
            ClientSocket = socket;
        }

        public void Close()
        {
            ClientSocket.Close();
        }



    }
}
