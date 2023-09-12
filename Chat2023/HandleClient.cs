using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat2023
{
    internal class HandleClient
    {
        private TcpClient clientSocket;
        private string clientName;

        public void StartClient(TcpClient client, string name)
        {
            clientSocket = client;
            clientName = name;
            var thread = new Thread(DoChat);
            thread.Start();
        }

        private void DoChat()
        {
            while (true)
            {
                try
                {
                    string dataFromClient = clientSocket.ReadString();
                    Program.Broadcast(dataFromClient, clientName, true);
                    Console.WriteLine($"{clientName} said: {dataFromClient}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    }
}
