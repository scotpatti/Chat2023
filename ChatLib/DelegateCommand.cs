using System.Windows.Input;

namespace ChatLib;

public class DelegateCommand : ICommand
{
    private readonly Predicate<object?> _canExecute;
    private readonly Action<object?> _execute;
    private bool flagExecutable = false;

    public event EventHandler? CanExecuteChanged;

    public DelegateCommand(Action<object?> execute, Predicate<object?> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        bool flag = _canExecute == null || _canExecute(parameter);
        if (flagExecutable != flag)
        {
            flagExecutable = !flagExecutable;
            RaiseCanExecuteChanged();
        }
        return flag;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged != null)
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
