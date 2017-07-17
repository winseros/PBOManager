using System;
using System.Threading;
using System.Windows.Input;

namespace PboManager.Components.PboTree.NodeMenu
{
    public abstract class NodeMenuItemModel : ViewModel
    {
        private readonly Lazy<string> text;
        private readonly Lazy<ICommand> command;

        protected NodeMenuItemModel(PboNodeModel node)
        {
            this.Node = node;
            this.text = new Lazy<string>(this.GetMenuItemText, LazyThreadSafetyMode.None);
            this.command = new Lazy<ICommand>(() => new Command(this.HandleExecute, this.CanExecute), LazyThreadSafetyMode.None);
        }

        public string Text => this.text.Value;

        public ICommand Command => this.command.Value;

        protected PboNodeModel Node { get; }

        protected abstract string GetMenuItemText();

        protected abstract void HandleExecute(object param);

        protected virtual bool CanExecute(object param)
        {
            return true;
        }
    }
}