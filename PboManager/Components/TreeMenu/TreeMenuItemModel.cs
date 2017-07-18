using System;
using System.Threading;
using System.Windows.Input;

namespace PboManager.Components.TreeMenu
{
    public abstract class TreeMenuItemModel : ViewModel
    {
        private readonly Lazy<string> text;
        private readonly Lazy<ICommand> command;

        protected TreeMenuItemModel()
        {
            this.text = new Lazy<string>(this.GetMenuItemText, LazyThreadSafetyMode.None);
            this.command = new Lazy<ICommand>(() => new Command(this.HandleExecute), LazyThreadSafetyMode.None);
        }

        public string Text => this.text.Value;

        public ICommand Command => this.command.Value;

        protected abstract string GetMenuItemText();

        protected abstract void HandleExecute(object param);
    }
}