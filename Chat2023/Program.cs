using ChatLib;
using System.Net;
using System.Net.Sockets;

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
                ChatMessage joinMessage = clientSocket.ReadChatMessage();
                ClientList.Add(joinMessage.Sender, clientSocket);
                Broadcast(new ChatMessage("System", $"{joinMessage.Sender} joined the chat."));
                var client = new HandleClient(clientSocket, joinMessage.Sender);
                client.StartClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client aborted connection: " + ex.Message);
            }
        }
    }

    public static void Broadcast(ChatMessage msg)
    {
        foreach (var item in ClientList)
        {
            try { 
                item.Value.WriteChatMessage(msg);
            }
            catch
            {
                Console.WriteLine($"Unable to write to user {item.Key}, removing user.");
                ClientList.Remove(item.Key);
            }
        }
    }
    

}