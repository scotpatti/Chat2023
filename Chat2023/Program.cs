using System.Net.Sockets;
using System;
using System.Net;
using Extensions;

namespace Chat2023;

public static class Program
{
    public static Dictionary<string, TcpClient> ClientList
        = new Dictionary<string, TcpClient>();
    public static void Main()
    {
        var ServerSocket = new TcpListener(IPAddress.Any, 8888);
        ServerSocket.Start();
        Console.WriteLine("Chat server has started");
        while (true)
        {
            try
            {
                var clientSocket = ServerSocket.AcceptTcpClient();
                string username = clientSocket.ReadString().TrimEnd();
                ClientList.Add(username, clientSocket);
                Broadcast(" joined the chatroom.", username, false);
                var client = new HandleClient();
                client.StartClient(clientSocket, username);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client aborted connection");
            }
        }
    }

    public static void Broadcast(string msg, string uname, bool flag)
    {
        foreach (var item in ClientList)
        {
            try { 
                var m = flag ? uname + " says: " + msg : uname + msg + "\r\n";
                item.Value.WriteString(m);
            }
            catch
            {
                Console.WriteLine($"Unable to write to {item.Key}");
                ClientList.Remove(item.Key);
            }
        }
    }
    

}