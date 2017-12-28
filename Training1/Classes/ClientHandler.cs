using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training1.Classes
{
    internal class ClientHandler //internal class of server. each clienthandler handles one client
    {
        private Socket clientSocket;
        private const string endMsg = "@quit";

        public Socket ClientSocket //clientsocket stores the socket of the client from serversocket.accept
        {
            get { return clientSocket; }
            private set { clientSocket = value; }
        }

        public ClientHandler(Socket socket) //construct new clienthandler for a socket of a new client
        {
            ClientSocket = socket; //store new client socket in the property
        }

        public void Close()
        {
            Send(endMsg);
            ClientSocket.Close(1); //close clientsocket if needed (when server closes the connection)
        }

        public void Send(string update)
        {
            //send client a message with button and new state "button:state"
            //for example "1:red" or "4:green"
            //the client has to process this message and update his gui accordingly
            //client can use the same guiUpdater in the mainVM as the server 
            //(because they use the same view and the things to be updated are also the same)
            ClientSocket.Send(Encoding.UTF8.GetBytes(update));
        }


    }
}
