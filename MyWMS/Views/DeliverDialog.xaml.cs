using MahApps.Metro.Controls;
using MyWMS.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace MyWMS.Views
{
    public partial class DeliverDialog : MetroWindow
    {
        public ObservableCollection<Warehouse> Warehouses { get; set; }
        private readonly DealDetail owner;
        public DeliverDialog(DealDetail owner)
        {
            this.owner = owner;
            Warehouses = new ObservableCollection<Warehouse>();
            using var db = MyDbContext.Instance;
            foreach (var i in db.Warehouses)
            {
                if (i.Available)
                    Warehouses.Add(i);
            }
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new InfoDialog("这回自动产生一条出库记录以及一条入库记录，您确定吗？", true)
            {
                Ok = async () =>
                {
                    await owner.VM.Commit(((FromWarehosueSplitBtn.SelectedItem as Warehouse).Id, true), default);
                    await owner.VM.Commit(((ToWarehouseSplitBtn.SelectedItem as Warehouse).Id, false), default);
                }
            }.Show();
        }
    }
}
