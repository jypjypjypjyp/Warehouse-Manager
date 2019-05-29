using System;
using System.Windows.Input;

namespace MyWMS.Helpers
{
    public class DelegateCommand : ICommand
    {
        public static DelegateCommand Create(Action<object> executeAction, Func<object, bool> canExecuteAction = null)
        {
            return new DelegateCommand(executeAction, canExecuteAction);
        }

        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction = null)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public void Execute(object parameter) => _executeAction(parameter);

        public bool CanExecute(object parameter) => _canExecuteAction?.Invoke(parameter) ?? true;

        public event EventHandler CanExecuteChanged;

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
