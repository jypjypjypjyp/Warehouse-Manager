using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class KeeperInfoView : UserControl
    {
        private KeeperInfoViewModel vm;
        public KeeperInfoView()
        {
            vm = new KeeperInfoViewModel(this);
            DataContext = vm;
            InitializeComponent();
            vm.Init();
        }

        private void SetPassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new SetPasswordDialog(this).Show();
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.Owner.ShowLogin();
        }
    }
}
