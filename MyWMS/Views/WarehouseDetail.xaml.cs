using MyWMS.Helpers;
using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class WarehouseDetail : UserControl
    {
        public WarehouseView Owner { get; set; }

        public WarehouseDetailViewModel VM { get; set; }
        public WarehouseDetail()
        {
            VM = new WarehouseDetailViewModel(this);
            DataContext = VM;
            InitializeComponent();
        }

        private void ToDeal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.Owner.Navigate(TabViewType.Deal, (TabViewType.Warehouse, VM.Id));
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Owner.CloseDetail();
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new NewWarehouseEntryDialog(this).Show();
        }

        private void Print_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new WarehousePrintDialog(this).Show();
        }
    }
}
