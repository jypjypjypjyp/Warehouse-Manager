using MyWMS.Models;
using MyWMS.Views;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyWMS.Helpers;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class WarehouseDetailViewModel : ViewModelBase
    {
        #region Properties
        public string Title
        {
            get => "仓库： " + _Id;
        }
        private int _Id;
        public int Id
        {
            get => _Id;
            set
            {
                SetProperty(ref _Id, value, "Title");
                SetProperty(ref _Id, value, "Id");
            }
        }

        public ObservableCollection<WarehouseEntry> WarehouseEntries { get; set; }
        #endregion

        #region Command
        public ICommand ConfirmCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand AbandonCommand { get; set; }
        #endregion


        private readonly WarehouseDetail owner;
        public WarehouseDetailViewModel(WarehouseDetail owner)
        {
            this.owner = owner;
            WarehouseEntries = new ObservableCollection<WarehouseEntry>();
            ConfirmCommand = DelegateCommand.Create(Commit);
            ResetCommand = DelegateCommand.Create(o => InitAsync(Id));
            AbandonCommand = DelegateCommand.Create(Abandon);
        }

        public async void InitAsync(int id)
        {
            WarehouseEntries.Clear();
            Id = id;
            IEnumerable<WarehouseEntry> allWarehouse = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allWarehouse = db.WarehouseEntries.Include(a => a.Item).Where(a => a.WarehouseId == id).AsEnumerable();
            });
            foreach (var i in allWarehouse)
            {
                WarehouseEntries.Add(i);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }

        public void AddEntry(Item item, double amount)
        {
            WarehouseEntries.Add(new WarehouseEntry()
            {
                Item = item,
                ItemId = item.Id,
                WarehouseId = Id,
                Amount = amount
            });
            MainWindowViewModel.Instance.StatusText = "添加成功！";
        }

        public async void Commit(object p)
        {
            var dict = new Dictionary<int, double>();
            foreach (var i in WarehouseEntries)
            {
                if (dict.TryGetValue(i.ItemId, out double curAmount))
                {
                    dict[i.ItemId] = curAmount + i.Amount;
                }
                else
                {
                    dict[i.ItemId] = i.Amount;
                }
            }
            await Task.Run(() =>
            {
                using var db = MyDbContext.Instance;
                foreach (var i in dict)
                {
                    if (i.Value < 0)
                    {
                        owner.Dispatcher.Invoke(owner.DataIncorrect);
                        return;
                    }
                    else
                    {

                        var entry = db.WarehouseEntries.Find(Id, i.Key);
                        if (i.Value > 0)
                        {
                            if (entry == null)
                            {
                                db.WarehouseEntries.Add(
                                    new WarehouseEntry()
                                    {
                                        ItemId = i.Key,
                                        WarehouseId = Id,
                                        Amount = i.Value
                                    });
                            }
                            else
                            {
                                entry.Amount = i.Value;
                                db.WarehouseEntries.Update(entry);
                            }
                        }
                        else if (i.Value == 0 && entry != null)
                        {
                            db.WarehouseEntries.Remove(entry);
                        }

                    }
                }
                db.SaveChanges();
            });
            owner.Owner.CloseDetail();
            MainWindowViewModel.Instance.StatusText = "更新成功！";
        }

        private async void Abandon(object p)
        {
            using var db = MyDbContext.Instance;
            db.Warehouses.Update(new Warehouse() { Id = Id, Available = false });
            await db.SaveChangesAsync();
            owner.Owner.CloseDetail();
            owner.Owner.Refresh();
            MainWindowViewModel.Instance.StatusText = "弃用成功！";
        }
    }
}
