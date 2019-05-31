using MahApps.Metro.Controls;

namespace MyWMS.Views
{
    public partial class NewKeeperDialog : MetroWindow
    {
        private readonly KeeperView owner;
        public NewKeeperDialog(KeeperView owner)
        {
            this.owner = owner;
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NameBox.Text == null || NameBox.Text == "")
            {
                new InfoDialog("请填写姓名！", false).Show();
                return;
            }
            owner.VM.AddKeeper(NameBox.Text, ContactBox.Text);
            new InfoDialog("请记住初始密码：123", false).Show();
            Close();
        }
    }
}
