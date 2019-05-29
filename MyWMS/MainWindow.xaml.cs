using MahApps.Metro.Controls;
using MyWMS.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyWMS
{
    public enum ViewType
    {
        Warehouse, Deal, Item, Saleman, Keeper, KeeperInfo
    }
    public partial class MainWindow : MetroWindow
    {
        public double RealTimeWidth { get; set; }
        public double RealTimeHeight { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RealTimeHeight = ActualHeight;
            RealTimeWidth = ActualWidth;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RealTimeHeight = e.NewSize.Height;
            RealTimeWidth = e.NewSize.Width;
        }
        public MainWindow()
        {
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            DataContext = MainWindowViewModel.Instance;
            MainWindowViewModel.Instance.Owner = this;
            InitializeComponent();
            LayoutTransform = new ScaleTransform(1.5, 1.5);
        }

        public void ShowMenu()
        {
            LoginView.Visibility = Visibility.Collapsed;
            ViewControl.Visibility = Visibility.Visible;
        }
        public void ShowLogin()
        {
            LoginView.Visibility = Visibility.Visible;
            ViewControl.Visibility = Visibility.Collapsed;
        }
        public void SetName(string name)
        {
            KeeperInfoTab.Header = name;
        }

        public void Navigate(ViewType type, object p = null)
        {
            MainWindowViewModel.Instance.Parameter = p;
            switch (type)
            {
                case ViewType.Warehouse:
                    ViewControl.SelectedItem = WarehouseTab;
                    break;
                case ViewType.Deal:
                    ViewControl.SelectedItem = DealTab;
                    break;
                case ViewType.Item:
                    ViewControl.SelectedItem = ItemTab;
                    break;
                case ViewType.Saleman:
                    ViewControl.SelectedItem = SalemanTab;
                    break;
                case ViewType.Keeper:
                    ViewControl.SelectedItem = KeeperTab;
                    break;
                case ViewType.KeeperInfo:
                    ViewControl.SelectedItem = KeeperInfoTab;
                    break;
                default:
                    break;
            }
        }
    }
}
