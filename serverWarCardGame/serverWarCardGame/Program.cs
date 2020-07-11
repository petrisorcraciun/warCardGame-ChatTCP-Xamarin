using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace serverWarCardGame
{
    class Program
    {
        public static TcpClient client;
        private static TcpListener listener;
        private static string ipString;

        static Server server;
        static Thread listenerThread;


        static void Main(string[] args)
        {


            try
            {
                server = new Server();
                listenerThread = new Thread(server.Listen);
                listenerThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                server.Disconnect();
                Console.ReadKey();
            }


            /*
          
            try
            {
                IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress address in localIp)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipString = address.ToString();
                    }
                }




                IPAddress ipAd = IPAddress.Parse(ipString);

                TcpListener myList = new TcpListener(ipAd, 5150);

                myList.Start();

                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("The local End point is  :" +
                                  myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                while (true)
                {
                    Socket s = myList.AcceptSocket();

                    ThreadPool.QueueUserWorkItem(ThreadProc, s);
                    
                    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                    byte[] b = new byte[100];
                    while (true)
                    {
                        int k = s.Receive(b);
                        Console.WriteLine("Recieved...");
                        for (int i = 0; i < k; i++)
                            Console.Write(Convert.ToChar(b[i]));

                        ASCIIEncoding asen = new ASCIIEncoding();
                        s.Send(asen.GetBytes("The string was recieved by the server."));
                        Console.WriteLine("\nSent Acknowledgement");
                        //s.Close();

                        if (k == 0)
                        {
                            s.Close();
                            break;
                        }
                    }
                    
                }
                //myList.Stop();
                //Console.ReadLine();
            }
            catch (Exception e)
            {

            }

            /*
            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipString = address.ToString();
                }
            }

          

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipString), 1234);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine(@"    
            ===================================================    
                   Started listening requests at: {0}:{1}    
            ===================================================",
            ep.Address, ep.Port);
            // client = listener.AcceptTcpClient();

            List<TcpClient> listConnectedClients = new List<TcpClient>();

            while (true) // Add your exit flag here
            {
                client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ThreadProc, client);
                Console.WriteLine("Connected to client!" + " \n");
                listConnectedClients.Add(client);


                const int bytesize = 1024 * 1024;
                byte[] buffer = new byte[bytesize];
                string x = client.GetStream().Read(buffer, 0, bytesize).ToString();
                var data = ASCIIEncoding.ASCII.GetString(buffer);

                if (data.ToUpper().Contains("PLAYER1"))
                {
                    Console.WriteLine("PRESS!" + " \n");

                }


            }
            */

            /*
           while (client.Connected)
           {


                    try
                    {
                        const int bytesize = 1024 * 1024;
                        byte[] buffer = new byte[bytesize];
                        string x = client.GetStream().Read(buffer, 0, bytesize).ToString();
                        var data = ASCIIEncoding.ASCII.GetString(buffer);

                        if (data.ToUpper().Contains("PLAYER1"))
                        {
                            Console.WriteLine("PRESS!" + " \n");

                        }



                    }
                    catch (Exception exc)
                    {
                        client.Dispose();
                        client.Close();
                    }

        


                     NetworkStream nwStream = client.GetStream();
                     byte[] buffer = new byte[client.ReceiveBufferSize];

                     int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);


                     string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                     String players = String.Empty;

                     players += " " + dataReceived;



                     Console.WriteLine("Received : " + dataReceived);


                     Console.WriteLine("Sending back : " + dataReceived);
                     //nwStream.Write(buffer, 0, bytesToSend);

                     byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(players);

                     //---send the text---
                     Console.WriteLine("Sending : " + players);
                     nwStream.Write(bytesToSend, 0, players.Length);

       

            }
               */


        }

        private static void ThreadProc(object obj)
        {
            var client = (Socket)obj;
            // Do your work here
        }

       

    }
}
