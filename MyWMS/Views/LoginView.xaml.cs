using MyWMS.ViewModels;
using System.Windows.Controls;

namespace MyWMS.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            DataContext = new LoginViewModel(this);
            InitializeComponent();
        }

        public void Success()
        {
            MainWindowViewModel.Instance.Owner.ShowMenu();
        }
        public void Failed()
        {
            new InfoDialog("用户名或密码错误", false).Show();
            MainWindowViewModel.Instance.StatusText = "用户名或密码错误！";
        }
    }
}
