using MahApps.Metro.Controls;
using MyWMS.Helpers;
using MyWMS.ViewModels;
using MyWMS.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyWMS
{
    public partial class MainWindow : MetroWindow
    {
        public double Zoom { get; set; }
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
            Task.Run(() =>
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory, "*.xps", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                    File.Delete(file);
            });
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            DataContext = MainWindowViewModel.Instance;
            MainWindowViewModel.Instance.Owner = this;
            InitializeComponent();
            Zoom = 1;
            LayoutTransform = new ScaleTransform(Zoom, Zoom);
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
                case TabViewType.Customer:
                    ViewControl.SelectedItem = CustomerTab;
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
            ViewControl.FindVisualChildren<TabView>().FirstOrDefault()?.Init(p);
        }
    }
}
