using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action<object> _executeWithParam;
    private readonly Action _executeWithoutParam;
    private readonly Func<object, bool> _canExecuteWithParam;
    private readonly Func<bool> _canExecuteWithoutParam;

    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecuteWithParam = canExecute;
    }

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _executeWithoutParam = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecuteWithoutParam = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        if (_canExecuteWithParam != null)
        {
            return _canExecuteWithParam(parameter);
        }

        return _canExecuteWithoutParam == null || _canExecuteWithoutParam();
    }

    public void Execute(object parameter)
    {
        if (_executeWithParam != null)
        {
            _executeWithParam(parameter);
        }
        else
        {
            _executeWithoutParam?.Invoke();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
