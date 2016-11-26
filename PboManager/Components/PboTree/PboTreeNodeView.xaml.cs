using System.Windows.Controls;
using ReactiveUI;

namespace PboManager.Components.PboTree
{    
    public partial class PboTreeNodeView : StackPanel, IViewFor<PboTreeNode>
    {
        public PboTreeNodeView()
        {
            this.InitializeComponent();
            this.OneWayBind(this.ViewModel, node => node.Name, view => view.TextName.Text);
            this.OneWayBind(this.ViewModel, node => node.Icon, view => view.Icon.Source);
        }

        #region IViewFor

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        public PboTreeNode ViewModel
        {
            get { return (PboTreeNode)this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion
    }
}
