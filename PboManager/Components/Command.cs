using System;
using System.Windows.Input;

namespace PboManager.Components
{
    public class Command : ICommand
    {
        private bool canExecute = true;
        private readonly Func<object, bool> handleCanExecute;
        private readonly Action<object> handleExecute;

        public Command(Action<object> handleExecute)
        {
            this.handleExecute = handleExecute;
        }

        public Command(Action<object> handleExecute, Func<object, bool> handleCanExecute)
        {
            this.handleExecute = handleExecute;
            this.handleCanExecute = handleCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.handleCanExecute != null)
            {
                bool can = this.handleCanExecute(parameter);
                if (this.canExecute != can)
                {
                    this.canExecute = can;
                    this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            return this.canExecute;
        }

        public void Execute(object parameter)
        {
            if (this.canExecute)
                this.handleExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}