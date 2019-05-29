using MahApps.Metro.Controls;
using MyWMS.Models;
using System.Collections.ObjectModel;

namespace MyWMS.Views
{
    public partial class NewWarehouseEntryDialog : MetroWindow
    {
        private readonly WarehouseDetail owner;
        public ObservableCollection<Item> Items { get; set; }
        public NewWarehouseEntryDialog(WarehouseDetail owner)
        {
            this.owner = owner;
            Items = new ObservableCollection<Item>();
            using var db = MyDbContext.Instance;
            foreach (var i in db.Items)
            {
                if(i.Available)
                    Items.Add(i);
            }
            InitializeComponent();
            MessageLabel.Content = "仓库" + owner.Id;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ItemSplitBtn.SelectedItem == null || AmountNumUD.Value == null)
            {
                new InfoDialog("请检查填写！", false).Show();
                return;
            }
            var item = ItemSplitBtn.SelectedItem as Item;
            double amount = (double)AmountNumUD.Value;
            owner.AddEntry(item, amount);
            Close();
        }
    }
}
