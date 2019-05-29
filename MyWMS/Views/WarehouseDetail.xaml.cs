using MyWMS.ViewModels;
using System.Windows.Controls;
using MyWMS.Models;

namespace MyWMS.Views
{
    public partial class WarehouseDetail : UserControl
    {
        public WarehouseView Owner { get; set; }
        public int Id { get; set; }

        private WarehouseDetailViewModel vm;
        public WarehouseDetail()
        {
            vm = new WarehouseDetailViewModel(this);
            DataContext = vm;
            InitializeComponent();
        }

        public void InitAsync(int id)
        {
            Id = id;
            vm.InitAsync(id);
        }

        public void AddEntry(Item item, double amount)
        {
            vm.AddEntry(item, amount);
        }

        private void ToDeal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.Owner.Navigate(ViewType.Deal);
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Owner.CloseDetail();
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new NewWarehouseEntryDialog(this).Show();
        }

        private void Reset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.InitAsync(Id);
        }

        public void DataIncorrect()
        {
            new InfoDialog("数量不能为复数！", true)
            {
                Cancel = () => Reset_Click(null, null)
            }.Show();
        }
    }
}
