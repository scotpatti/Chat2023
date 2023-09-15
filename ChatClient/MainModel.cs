﻿using ChatLib;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;


namespace ChatClient;

public class MainModel : INotifyPropertyChanged
{
    #region Properties and Fields
    public readonly string IP = Extensions.LocalIPAddress().ToString();
    public readonly int PORT = 8888;

    private TcpClient? _socket;

    private string _Username;
    public string Username
    {
        get => _Username; 
        set => _Username = SetField<string>(value); 
    }

    private string _MessageBoard;
    public string MessageBoard { 
        get => _MessageBoard;
        set => _MessageBoard = SetField<string>(value); 
    }

    private string _CurrentMessage;
    public string CurrentMessage
    {
        get => _CurrentMessage;
        set => _CurrentMessage = SetField<string>(value);
    }

    private bool _Connected;
    public bool Connected
    {
        get => _Connected; 
        set => _Connected = SetField<bool>(value);
    }
    #endregion

    #region Constructor
    public MainModel()
    {
        _Username = string.Empty;
        _CurrentMessage = string.Empty;
        _MessageBoard = string.Empty;
        _Connected = false;
    }
    #endregion

    #region Methods
    public void Connect()
    {
        _socket = new TcpClient();
        _socket.Connect(IP, PORT);
        Connected = true;
        Send();
        var thread = new Thread(GetMessage);
        thread.Start();
    }

    public void Send()
    {
        if (_socket != null)
        {
            ChatMessage msg = new ChatMessage(_Username, _CurrentMessage);
            _socket.WriteChatMessage(msg);
        }
    }

    public void GetMessage()
    {
        while (_socket != null)
        {
            ChatMessage? msg = _socket.ReadChatMessage();
            if (msg!=null)
            {
                MessageBoard +=$"{msg.Sender} says: {msg.Message}\r\n";
            }
            
        }
    }
        

    

    #endregion

    #region INPC
    public event PropertyChangedEventHandler? PropertyChanged;

    protected T SetField<T>(T value, [CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return value;
    }
    #endregion
}

