using System.Windows;
using PboManager.Components.Converters;
using ReactiveUI;
using Util;

namespace PboManager.Components.MainWindow
{
    public partial class MainWindow : Window, IViewFor<MainWindowModel>
    {
        public MainWindow(MainWindowModel mainWindowModel)
        {
            Assert.NotNull(mainWindowModel, nameof(mainWindowModel));
            this.DataContext = mainWindowModel;
            this.InitializeComponent();

            this.OneWayBind(this.ViewModel, model => model.Title, window => window.Title, TextToWindowTitle.Convert);
        }

        #region IViewFor

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        public MainWindowModel ViewModel
        {
            get { return (MainWindowModel) this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion
    }
}