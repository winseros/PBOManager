using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace PboManager.Components.TreeMenu
{
    public abstract class TreeMenuModel : ViewModel
    {
        private readonly Lazy<ObservableCollection<TreeMenuItemModel>> items;

        protected TreeMenuModel()
        {
            this.items = new Lazy<ObservableCollection<TreeMenuItemModel>>(this.GetItems, LazyThreadSafetyMode.None);
        }

        protected abstract ObservableCollection<TreeMenuItemModel> GetItems();

        public ObservableCollection<TreeMenuItemModel> Items => this.items.Value;
    }
}