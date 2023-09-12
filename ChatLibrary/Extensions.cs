﻿using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System;
using ChatLibrary;

namespace ChatLibrary
{
    public static class Extensions
    {
        public static string ReadString(this TcpClient client)
        {
            StringBuilder sb = new StringBuilder();
            var bytes = new byte[client.ReceiveBufferSize];
            var stream = client.GetStream();
            bool EndOfMessageFound = false;
            while (!EndOfMessageFound)
            {
                stream.Read(bytes, 0, bytes.Length);
                string jMsg = Encoding.ASCII.GetString(bytes);
                if (jMsg.Contains('\0', StringComparison.Ordinal))
                {
                    EndOfMessageFound = true;
                    sb.Append(jMsg.Substring(0, jMsg.IndexOf('\0', StringComparison.Ordinal)));
                }
                else
                {
                    sb.Append(jMsg);
                }
            }
            return sb.ToString();
        }

        public static void WriteString(this TcpClient client, string msg)
        {
            var bytes = Encoding.ASCII.GetBytes(msg);
            var stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

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
}