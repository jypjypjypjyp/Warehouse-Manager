using MyWMS.Helpers;
using MyWMS.ViewModels;

namespace MyWMS.Views
{
    public partial class CustomerView : TabView
    {
        public CustomerViewModel VM { get; set; }
        public CustomerView()
        {
            VM = new CustomerViewModel(this);
            DataContext = VM;
            InitializeComponent();
            VM.InitAsync();
            Init(null);
        }

        public override void Init(object p)
        {
        }

        public void ToDeal(object p)
        {
            int id = (int)p;
            MainWindowViewModel.Instance.Owner.Navigate(TabViewType.Deal, (TabViewType.Customer, id));
        }
    }
}
