using MyWMS.Helpers;
using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class DealDetail : UserControl
    {
        public DealView Owner { get; set; }
        public int Id { get; set; }

        public DealDetailViewModel VM { get; set; }
        public DealDetail()
        {
            VM = new DealDetailViewModel(this);
            DataContext = VM;
            InitializeComponent();
        }

        public void InitAsync(bool editable, int id)
        {
            Id = id;
            VM.InitAsync(id);
        }

        private void ToDeal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.Owner.Navigate(TabViewType.Deal, (TabViewType.Warehouse, Id));
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Owner.CloseDetail();
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new NewWarehouseEntryDialog(this).Show();
        }

        public void DataIncorrect()
        {
            new InfoDialog("数量不能为复数！", true)
            {
                Cancel = () => VM.InitAsync(Id)
            }.Show();
        }
    }
}
