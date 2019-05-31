using MyWMS.Helpers;
using MyWMS.ViewModels;

namespace MyWMS.Views
{
    public partial class SalemanView : TabView
    {
        public SalemanViewModel VM { get; set; }
        public SalemanView()
        {
            VM = new SalemanViewModel(this);
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
            MainWindowViewModel.Instance.Owner.Navigate(TabViewType.Deal, (TabViewType.Salesman, id));
        }
    }
}
