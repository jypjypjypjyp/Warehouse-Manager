using MahApps.Metro.Controls;
using MyWMS.Helpers;
using MyWMS.ViewModels;
using MyWMS.Views;
using System.Windows;
using System.Windows.Media;

namespace MyWMS
{
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

        public void Navigate(TabViewType type, object p = null)
        {
            switch (type)
            {
                case TabViewType.Warehouse:
                    ViewControl.SelectedItem = WarehouseTab;
                    break;
                case TabViewType.Deal:
                    ViewControl.SelectedItem = DealTab;
                    break;
                case TabViewType.Item:
                    ViewControl.SelectedItem = ItemTab;
                    break;
                case TabViewType.Salesman:
                    ViewControl.SelectedItem = SalemanTab;
                    break;
                case TabViewType.Keeper:
                    ViewControl.SelectedItem = KeeperTab;
                    break;
                case TabViewType.KeeperInfo:
                    ViewControl.SelectedItem = KeeperInfoTab;
                    break;
                default:
                    break;
            }
            ViewControl.FindChild<TabView>().Init(p);
        }
    }
}
