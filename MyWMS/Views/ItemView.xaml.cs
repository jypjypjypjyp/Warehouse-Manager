using MyWMS.Helpers;
using MyWMS.ViewModels;

namespace MyWMS.Views
{
    public partial class ItemView : TabView
    {
        public ItemViewModel VM { get; set; }
        public ItemView()
        {
            VM = new ItemViewModel(this);
            DataContext = VM;
            InitializeComponent();
            VM.InitAsync();
            Init(null);
        }

        public void DataIncorrect()
        {
            new InfoDialog("输入有误!", true)
            {
                Cancel = VM.InitAsync
            }.Show();
        }

        public override void Init(object p)
        {
        }
    }
}
