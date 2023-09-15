using ChatLib;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient;

public class MainViewModel : INotifyPropertyChanged
{
    #region Properties, Fields and Variables
    private readonly MainModel _mainModel;

    public string Username
    {
        get => _mainModel.Username;
        set => _mainModel.Username = SetField<string>(value);
    }

    public string Message
    {
        get => _mainModel.CurrentMessage;
        set => _mainModel.CurrentMessage = SetField<string>(value); 
    }

    public string MessageBoard
    {
        get => _mainModel.MessageBoard; 
        set => _mainModel.MessageBoard = SetField<string>(value);
    }

    public DelegateCommand ConnectCommand { get; set; }
    public DelegateCommand SendCommand { get; set; }

    #endregion

    #region Constructor
    public MainViewModel()
    {
        _mainModel = new MainModel();
        _mainModel.PropertyChanged += MainModelChanged;
        ConnectCommand = new DelegateCommand(
            a => { _mainModel.Connect(); ConnectCommand?.RaiseCanExecuteChanged(); SendCommand?.RaiseCanExecuteChanged(); },
            b => !_mainModel.Connected);
        SendCommand = new DelegateCommand(
            a => _mainModel.Send(),
            b => _mainModel.Connected);
    }
    #endregion

    #region Event Listeners
    private void MainModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName)) return;
        if (e.PropertyName.Equals("Connected"))
        {
            NotifyPropertyChanged("Connected");
            ConnectCommand.RaiseCanExecuteChanged();
            SendCommand.RaiseCanExecuteChanged();
        }
        else if (e.PropertyName.Equals("MessageBoard"))
        {
            NotifyPropertyChanged("MessageBoard");
        }
    }

    #endregion

    #region INPC
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged(string prop)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    protected T SetField<T>(T value, [CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return value;
    }
    #endregion
}
