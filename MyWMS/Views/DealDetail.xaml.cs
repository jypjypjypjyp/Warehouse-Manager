using MyWMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class DealDetail : UserControl
    {
        public DealView Owner { get; set; }

        public DealDetailViewModel VM { get; set; }
        public DealDetail()
        {
            VM = new DealDetailViewModel(this);
            DataContext = VM;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Owner.CloseDetail();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new NewDealEntryDialog(this).Show();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            new DealPrintDialog(this).Show();
        }

        private void Deliver_Click(object sender, RoutedEventArgs e)
        {
            new DeliverDialog(this).Show();
        }
    }
}
