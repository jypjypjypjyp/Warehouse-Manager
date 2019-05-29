using MyWMS.ViewModels;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Windows;

namespace MyWMS.Views
{
    public partial class WarehouseView : UserControl
    {
        private WarehouseViewModel vm;
        public WarehouseView()
        {
            SizeChanged += OnSizeChanged;
            vm = new WarehouseViewModel(this);
            DataContext = vm;
            InitializeComponent();
            WarehouseDetail.Owner = this;
            vm.InitAsync();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(WarehouseDetail, 0.1/1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth);
            Canvas.SetTop(WarehouseDetail, Math.Max(0.1/1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight - 50, 0));
            WarehouseDetail.Width = 0.8/1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth;
            WarehouseDetail.Height = 0.8/1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackDrop.Visibility = Visibility.Visible;
            WarehouseDetail.Visibility = Visibility.Visible;
            
            var button = sender as Button;
            var id = int.Parse(
                ((button.Content as Grid)
                .Children.Cast<UIElement>()
                .Where(a => a is TextBlock)
                .FirstOrDefault() as TextBlock)
                .Text);
            WarehouseDetail.InitAsync(id);
        }

        private void BackDrop_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CloseDetail();
        }

        public void CloseDetail()
        {
            BackDrop.Visibility = Visibility.Collapsed;
            WarehouseDetail.Visibility = Visibility.Collapsed;
        }

        public void Refresh()
        {
            vm.InitAsync();
        }

    }
}
