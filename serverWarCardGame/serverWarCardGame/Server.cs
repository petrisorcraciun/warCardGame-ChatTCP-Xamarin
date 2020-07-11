using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace serverWarCardGame
{
    
    class Server
    {
        public int nrPlayers = 2;
        static TcpListener listener;
        List<Client> clients = new List<Client>();
        private static string ipString;

        protected internal void AddConnection(Client client)
        {
            clients.Add(client);
        }

        protected internal void RemoveConnection(string guid)
        {
            Client client = clients.FirstOrDefault(c => c.guid == guid);
            if (client != null) clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress address in localIp)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipString = "192.168.100.17";

                       // Console.WriteLine(ipString);
                    }
                }

                listener = new TcpListener(IPAddress.Parse(ipString), 7777);
                listener.Start();
                Console.WriteLine("Waiting for connections...");
                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();

                    Client client = new Client(tcpClient, this);
                    Thread clientThread = new Thread(client.Process);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void BroadcastMessage(string message, string guid)
        {

            ASCIIEncoding asen2 = new ASCIIEncoding();
            byte[] data = asen2.GetBytes(message);

            // byte[] data = Encoding.Unicode.GetBytes(message);

            Console.WriteLine(nrPlayers + " " + clients.Count);

            for (int i = 0; i < clients.Count; i++)
            {
                        try
                        {
                            clients[i].stream.Write(data, 0, data.Length); //передача данных
                        }
                        catch (Exception ex)
                        {
                            clients[i].Close();
                            clients.RemoveAt(i);
                        }
            }

         
               
        }

        protected internal void Disconnect()
        {
            listener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }

    }
}
