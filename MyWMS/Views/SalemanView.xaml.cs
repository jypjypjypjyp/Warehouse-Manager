using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class SalemanView : UserControl
    {
        private SalemanViewModel vm;
        public SalemanView()
        {
            vm = new SalemanViewModel(this);
            DataContext = vm;
            InitializeComponent();
            vm.InitAsync();
        }

    }
}
