using MyWMS.Helpers;
using MyWMS.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class DealView : TabView
    {
        public DealViewModel VM { get; set; }
        public DealView()
        {
            SizeChanged += OnSizeChanged;
            VM = new DealViewModel(this);
            DataContext = VM;
            InitializeComponent();
            DealDetail.Owner = this;
            VM.InitAsync();
            Init(null);
        }

        public override async void Init(object p)
        {
            if (p == null) return;
            (TabViewType Type, int Id) t = ((TabViewType Type, int Id))p;
            await Task.Run(VM.InitFinishedEvent.WaitOne);
            switch (t.Type)
            {
                case TabViewType.Warehouse:
                    VM.SelectedWarehouse = VM.Warehouses.Where(a => a.Id == t.Id).FirstOrDefault();
                    break;
                case TabViewType.Salesman:
                    VM.SelectedSalesman = VM.Salesmen.Where(a => a.Id == t.Id).FirstOrDefault();
                    break;
                default:
                    break;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(DealDetail, 0.1 / 1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth);
            Canvas.SetTop(DealDetail, Math.Max(0.1 / 1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight - 50, 0));
            DealDetail.Width = 0.8 / 1.5 * MainWindowViewModel.Instance.Owner.RealTimeWidth;
            DealDetail.Height = 0.8 / 1.5 * MainWindowViewModel.Instance.Owner.RealTimeHeight;
        }

        public void ToDetail(object p)
        {
            BackDrop.Visibility = Visibility.Visible;
            DealDetail.Visibility = Visibility.Visible;

            (bool Editable, int Id) t = ((bool, int))p;
            DealDetail.InitAsync(t.Editable, t.Id);
        }

        private void BackDrop_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CloseDetail();
        }

        public void CloseDetail()
        {
            BackDrop.Visibility = Visibility.Collapsed;
            DealDetail.Visibility = Visibility.Collapsed;
        }
    }
}
