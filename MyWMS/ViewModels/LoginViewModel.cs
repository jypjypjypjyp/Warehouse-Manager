using MyWMS.Helpers;
using MyWMS.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private readonly LoginView owner;

        #region Commands
        public ICommand LoginCommand { get; private set; }
        #endregion

        public LoginViewModel(LoginView owner)
        {
            this.owner = owner;
            LoginCommand = DelegateCommand.Create(OnLogin);
        }

        private async void OnLogin(object parameter)
        {
            var values = (object[])parameter;
            int.TryParse((string)values[0], out int id);
            var password = (values[1] as PasswordBox).Password;
            using var db = MyDbContext.Instance;
            var k = await db.Keepers.FindAsync(id);
            if (k?.Password == password)
            {
                k.Password = null;
                MainWindowViewModel.Instance.CurKeeper = k;
                owner.Success();
            }
            else
                owner.Failed();
        }
    }
}
