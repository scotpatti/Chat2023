using ChatLib;
using System.Net.Sockets;

namespace Chat2023;

internal class HandleClient
{
    private TcpClient clientSocket;
    private string clientName;

    public HandleClient(TcpClient socket, string name)
    {
        if (socket is null || string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("TcpClient socket must not be null, and name must not be empty or whitespace.");
        }
        this.clientSocket = socket;
        this.clientName = name;
    }

    public void StartClient()
    {
        var thread = new Thread(DoChat);
        thread.Start();
    }

    private void DoChat()
    {
        while (true)
        {
            try
            {
                ChatMessage? msg = clientSocket.ReadChatMessage();
                if (msg != null)
                {
                    if (msg.Sender == clientName)
                    {
                        Program.Broadcast(msg);
                        Console.WriteLine($"{clientName} said: {msg.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"{clientName} did not get a message because it was ill formed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }
}
