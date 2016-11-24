using System.Windows.Controls;
using ReactiveUI;

namespace PboManager.Components.MainMenu
{    
    public partial class MainMenu : Menu,  IViewFor<MainMenuModel>
    {
        public MainMenu()
        {
            this.InitializeComponent();
            this.InitializeCommands();
        }
       
        private void InitializeCommands()
        {
            this.BindCommand(this.ViewModel, vm => vm.OpenFileCommand, v => v.MenuItemOpen);
        }

        #region IViewFor

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        public MainMenuModel ViewModel
        {
            get { return (MainMenuModel)this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion
    }
}
