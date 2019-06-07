using MahApps.Metro.Controls;
using MyWMS.Models;
using System.Collections.ObjectModel;

namespace MyWMS.Views
{
    public partial class NewDealEntryDialog : MetroWindow
    {
        private readonly DealDetail owner;
        public ObservableCollection<Item> Items { get; set; }
        public NewDealEntryDialog(DealDetail owner)
        {
            this.owner = owner;
            Items = new ObservableCollection<Item>();
            using var db = MyDbContext.Instance;
            foreach (var i in db.Items)
            {
                if (i.Available)
                    Items.Add(i);
            }
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ItemSplitBtn.SelectedItem == null || AmountNumUD.Value == null
                || PriceNumUD.Value == null || OffNumUD.Value == null)
            {
                new InfoDialog("请检查填写！", false).Show();
                return;
            }
            var item = ItemSplitBtn.SelectedItem as Item;
            double amount = (double)AmountNumUD.Value;
            double price = (double)PriceNumUD.Value;
            double off = (double)OffNumUD.Value;
            string note = NoteBox.Text;
            owner.VM.AddEntry(item, amount, price * (100.0 - off) / 100.0, note);
            Close();
        }
    }
}
