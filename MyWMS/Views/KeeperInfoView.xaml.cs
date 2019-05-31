using MyWMS.Helpers;
using MyWMS.ViewModels;

namespace MyWMS.Views
{
    public partial class KeeperInfoView : TabView
    {
        public KeeperInfoViewModel VM { get; set; }
        public KeeperInfoView()
        {
            VM = new KeeperInfoViewModel(this);
            DataContext = VM;
            InitializeComponent();
            VM.Init();
            Init(null);
        }

        private void SetPassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new SetPasswordDialog(this).Show();
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.Owner.ShowLogin();
        }

        public override void Init(object p)
        {
        }
    }
}
