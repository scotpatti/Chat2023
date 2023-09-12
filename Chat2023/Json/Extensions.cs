using Extensions;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Chat2023.Json;

public static class Extensions
{
    public static ChatMessage ReadChatMessage(this TcpClient client)
    {
        string msg = client.ReadString();
        (bool success, string errors) = JsonSchemaValidator.Validate(msg);
        if (!success) 
        {
            throw new Exception(errors);
        }
        var chatMessage = JsonConvert.DeserializeObject<ChatMessage>(msg);
        if (chatMessage == null)
        {
            throw new Exception("Something went wrong and we couldn't deserialize the object even though it passed validation!");
        }
        return chatMessage;
    }

    public static void WriteChatMessage(this TcpClient client, ChatMessage message)
    {
        string json = JsonConvert.SerializeObject(message);
        client.WriteString(json);
    }
}
