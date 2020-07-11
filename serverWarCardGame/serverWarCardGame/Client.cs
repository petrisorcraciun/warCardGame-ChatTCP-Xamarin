using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace serverWarCardGame
{
    class Client
    {
        protected internal string guid { get; private set; }
        protected internal NetworkStream stream { get; private set; }
        string userName;
        TcpClient tcpClient;
        Server server;

        String[] commands = { "startGame", "stopGame" };

        public Client(TcpClient tcpClient, Server server)
        {
            guid = Guid.NewGuid().ToString();
            this.tcpClient = tcpClient;
            this.server = server;
            server.AddConnection(this);
        }

        public void Process()
        {
            string message;
            try
            {
                stream = tcpClient.GetStream();
                userName = GetMessage();
                message = userName + " join the game.";
                server.BroadcastMessage(message, this.guid);
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        if (message != "")
                        {
                            if (commands.Any(x => x == message))
                            {
                                
                            }else
                            {
                                message = String.Format("{0}: {1}", userName, message);
                            }
                            Console.WriteLine(message);
                            server.BroadcastMessage(message, this.guid);
                        }
                    }
                    catch
                    {
                        message = String.Format("{0} leaves the game.", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.guid);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(this.guid);
                Close();
            }
        }

        protected internal void Close()
        {
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
        }
        private string GetMessage()
        {

            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);

            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            string message;
            while (stream.DataAvailable)
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            //message = builder.ToString();
            message = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            return message;
        }
    }
}
