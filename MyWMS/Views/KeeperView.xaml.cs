using MyWMS.Helpers;
using MyWMS.ViewModels;

namespace MyWMS.Views
{
    public partial class KeeperView : TabView
    {
        public KeeperViewModel VM { get; set; }
        public KeeperView()
        {
            VM = new KeeperViewModel(this);
            DataContext = VM;
            InitializeComponent();
            VM.InitAsync();
            Init(null);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new NewKeeperDialog(this).Show();
        }

        public override void Init(object p)
        {
        }
    }
}
