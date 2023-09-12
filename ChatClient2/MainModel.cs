using ChatLib;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
//using ChatHelper;

namespace ChatClient
{
    internal class MainModel : INotifyPropertyChanged
    {
        public readonly string IP = "10.10.12.13";
        public readonly int PORT = 8888;

        #region Properties and Fields
        private TcpClient _socket;

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { SetField<string>(ref _Username, value); }
        }

        private string _MessageBoard;
        public string MessageBoard { 
            get { return _MessageBoard;} 
            set { SetField<string>(ref _MessageBoard, value); } 
        }

        private string _CurrentMessage;
        public string CurrentMessage
        {
            get { return _CurrentMessage;}
            set { SetField<string>(ref _CurrentMessage, value); }
        }

        private bool _Connected;
        public bool Connected
        {
            get { return _Connected; }
            set { SetField<bool>(ref _Connected, value); }
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
            ChatMessage msg = new ChatMessage(_Username, _CurrentMessage);
            _socket.WriteChatMessage(msg);
        }

        public void GetMessage()
        {
            while (true)
            {
                ChatMessage msg = _socket.ReadChatMessage();
                MessageBoard += msg.Message + "\r\n";
            }
            

        }

        #endregion

        #region Constructor
        public MainModel()
        {
            _Connected = false;
        }
        #endregion

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
