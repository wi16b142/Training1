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
        private Socket serverSocket; //socket of server
        List<ClientHandler> clients = new List<ClientHandler>(); // list of clienthanders, each client has an own clienthandler
        Action<String> GuiUpdater; //delegate to function in mainVM to update server gui (e.g. if new incoming msg from client)
        Thread acceptingThread; //each client is accepted in an own thread

        public Server(string ip, int port, Action<string> guiUpdater)
        {
            this.GuiUpdater = guiUpdater; //the parameter delegate points to the function in the mainVM
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //create serversocket
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port)); //bind serversocket to NIC
            serverSocket.Listen(10); //server start listening (max. allowed clients = 10)
        }

        private void StartAccepting()
        {
            //accept clients in thread
            acceptingThread = new Thread(new ThreadStart(Accept)); //for each new client create thread
            acceptingThread.IsBackground = true; //run thread in background
            acceptingThread.Start(); //start this thread
        }

        private void Accept()
        {
            while (acceptingThread.IsAlive) //as long as the accepting thread is alive accept new clients
            {
                try
                {
                    clients.Add(new ClientHandler(serverSocket.Accept())); //create new clienthandler for new client and add it to the list
                }catch (Exception)
                {
                    //server not open
                }
            }
        }

        public void StopAccepting()
        {
            serverSocket.Close(); //close serversocket
            acceptingThread.Abort(); //abort the accepting thread

            foreach(var client in clients) //for each clienthandler (= client) disconnect the client from the server
            {
                client.Close(); //disconnect one client via: clienthandler.close
            }
            clients.Clear(); //empty list of clienthandler
        }

        private void BroadcastToggle(string button, string state)
        {
            string update = button + ":" + state; //create vlaue pair "button:state" e.g. "1:red"

            GuiUpdater(update); //points to a function in the mainVM to update the server GUI with the info in the form of a value pair "button:state"

            foreach (var client in clients) //broadcast button id and new state to each client in the form of a value pair "button:state"
            {
                client.Send(update);
            }

        }
    }
}
