using System.Windows.Input;

namespace WackEditor
{
    /// <summary>
    /// Editor command to be used by the undo/redo system,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class RelayCommand<T> : ICommand
    {

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;

        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        /// <summary>
        /// Creates a RelayCommand
        /// </summary>
        /// <param name="execute">The logic that executes when the command is called</param>
        /// <param name="canExecute">Optional predicate to check that controls if hte command is called or not</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;

        }

    }
}
