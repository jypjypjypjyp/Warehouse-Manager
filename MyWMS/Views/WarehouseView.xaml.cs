using MyWMS.ViewModels;
using System.Windows.Controls;
using System;
using System.Windows;
using MyWMS.Helpers;

namespace MyWMS.Views
{
    public partial class WarehouseView : TabView
    {
        public WarehouseViewModel VM { get; set; }
        public WarehouseView()
        {
            SizeChanged += OnSizeChanged;
            VM = new WarehouseViewModel(this);
            DataContext = VM;
            InitializeComponent();
            WarehouseDetail.Owner = this;
            VM.InitAsync();
            Init(null);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(WarehouseDetail, 0.1/1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth);
            Canvas.SetTop(WarehouseDetail, Math.Max(0.1/1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight - 50, 0));
            WarehouseDetail.Width = 0.8/1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth;
            WarehouseDetail.Height = 0.8/1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight;
        }

        public void ToDetail(object p)
        {
            BackDrop.Visibility = Visibility.Visible;
            WarehouseDetail.Visibility = Visibility.Visible;

            int id = (int)p;
            WarehouseDetail.VM.InitAsync(id);
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

        public override void Init(object p)
        {
            if (p == null) return;
            BackDrop.Visibility = Visibility.Visible;
            WarehouseDetail.Visibility = Visibility.Visible;
            var id = (int)p;
            WarehouseDetail.VM.InitAsync(id);
        }
    }
}
