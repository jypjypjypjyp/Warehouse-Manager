using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class WarehouseViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Warehouse> Warehouses { get; set; }
        #endregion

        #region Commands
        public ICommand AddCommand { get; set; }
        #endregion

        private int maxId;
        private readonly WarehouseView owner;
        public WarehouseViewModel(WarehouseView owner)
        {
            this.owner = owner;
            Warehouses = new ObservableCollection<Warehouse>();
            AddCommand = DelegateCommand.Create(Add);
        }

        public async void InitAsync()
        {
            Warehouses.Clear();
            IEnumerable<Warehouse> allWarehouse = null;
            await Task.Run(() =>
            {
                using var db = MyDbContext.Instance;
                allWarehouse = MyDbContext.Instance.Warehouses.AsEnumerable();
            });
            foreach (var i in allWarehouse)
            {
                if (i.Available)
                    Warehouses.Add(i);
                maxId = Math.Max(maxId, i.Id);
            }
        }

        private async void Add(object p)
        {
            var w = new Warehouse() { Id = ++maxId, Available = true };
            using var db = MyDbContext.Instance;
            db.Warehouses.Add(w);
            await db.SaveChangesAsync();
            Warehouses.Add(w);
        }
    }
}
