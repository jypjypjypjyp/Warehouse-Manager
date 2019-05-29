using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class ItemView : UserControl
    {
        private ItemViewModel vm;
        public ItemView()
        {
            vm = new ItemViewModel(this);
            DataContext = vm;
            InitializeComponent();
            vm.InitAsync();
        }

        public void DataIncorrect()
        {
            new InfoDialog("输入有误!", true)
            {
                Cancel = vm.InitAsync
            }.Show();
        }
    }
}
