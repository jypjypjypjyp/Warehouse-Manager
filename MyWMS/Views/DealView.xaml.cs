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
            await Task.Run(VM.InitFinishedEvent.WaitOne);
            if (p == null)
            {
                VM.UpdateFilter();
                return;
            }
            var t = ((TabViewType Type, int Id))p;
            switch (t.Type)
            {
                case TabViewType.Warehouse:
                    VM.SelectedWarehouse = VM.Warehouses.Where(a => a.Id == t.Id).FirstOrDefault();
                    break;
                case TabViewType.Salesman:
                    VM.SelectedSalesman = VM.Salesmen.Where(a => a.Id == t.Id).FirstOrDefault();
                    break;
                case TabViewType.Customer:
                    VM.SelectedCustomer = VM.Customers.Where(a => a.Id == t.Id).FirstOrDefault();
                    break;
                default:
                    break;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var zoom = MainWindowViewModel.Instance.Owner.Zoom;
            Canvas.SetLeft(DealDetail, 0.1 / zoom * MainWindowViewModel.Instance.Owner.RealTimeWidth);
            Canvas.SetTop(DealDetail, Math.Max(0.1 / zoom * MainWindowViewModel.Instance.Owner.RealTimeHeight - 50, 0));
            DealDetail.Width = 0.8 / zoom * MainWindowViewModel.Instance.Owner.RealTimeWidth;
            DealDetail.Height = 0.8 / zoom * MainWindowViewModel.Instance.Owner.RealTimeHeight;
        }

        public void ToDetail(object p)
        {
            BackDrop.Visibility = Visibility.Visible;
            DealDetail.Visibility = Visibility.Visible;

            (bool Editable, int Id) = ((bool, int))p;
            DealDetail.VM.InitAsync(Editable, Id);
        }

        public void ToSum(object p)
        {
            BackDrop.Visibility = Visibility.Visible;
            DealDetail.Visibility = Visibility.Visible;

            DealDetail.VM.InitAsync((int[]) p);
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
