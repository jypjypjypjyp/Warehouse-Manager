using MahApps.Metro.Controls;
using MyWMS.ViewModels;
using System.Windows;

namespace MyWMS.Views
{
    public partial class SetPasswordDialog : MetroWindow
    {
        private readonly KeeperInfoView owner;
        public SetPasswordDialog(KeeperInfoView owner)
        {
            this.owner = owner;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var db = MyDbContext.Instance;
            var i = db.Keepers.Find(MainWindowViewModel.Instance.CurKeeper.Id);
            if (NewPassword.Password != PasswordAgain.Password)
                goto Failed;
            if (OldPassword.Password != i.Password)
                goto Failed;
            i.Password = NewPassword.Password;
            db.Keepers.Update(i);
            db.SaveChanges();
            MainWindowViewModel.Instance.StatusText = "修改成功！";
            Close();
            return;
            Failed:
                new InfoDialog("密码错误！", false).Show();
        }
    }
}
