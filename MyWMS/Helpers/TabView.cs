using System.Windows.Controls;

namespace MyWMS.Helpers
{
    public interface ITabView
    {
        void Init(object p);
    }
    public enum TabViewType
    {
        Warehouse, Deal, Item, Salesman, Customer, Keeper, KeeperInfo
    }
    public abstract class TabView : UserControl, ITabView
    {
        public abstract void Init(object p);
    }
}
