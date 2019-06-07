using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public ICommand ConvertCommand { get; set; }
        #endregion

        private Dictionary<int, string> unitDict = new Dictionary<int, string>();
        private readonly WarehouseDetail owner;
        public WarehouseDetailViewModel(WarehouseDetail owner)
        {
            this.owner = owner;
            WarehouseEntries = new ObservableCollection<WarehouseEntry>();
            ConfirmCommand = DelegateCommand.Create(Commit);
            ResetCommand = DelegateCommand.Create(o => InitAsync(Id));
            AbandonCommand = DelegateCommand.Create(Abandon);
            ConvertCommand = DelegateCommand.Create(Convert);
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
                i.IsLoad = false;
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
            var progressDialog = new ProgressDialog(false);
            progressDialog.Show();
            try
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
                            //owner.Dispatcher.Invoke();
                            throw new Exception();
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
                                    db.WarehouseEntries.Update(db, entry);
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
            catch
            {
                new InfoDialog("数量不能为负数！", true)
                {
                    Cancel = () => InitAsync(Id)
                }.Show();
            }
            finally
            {
                progressDialog.Close();
            }
        }

        private async void Abandon(object p)
        {
            using var db = MyDbContext.Instance;
            db.Warehouses.Update(db, new Warehouse() { Id = Id, Available = false });
            await db.SaveChangesAsync();
            owner.Owner.CloseDetail();
            owner.Owner.VM.InitAsync();
            MainWindowViewModel.Instance.StatusText = "弃用成功！";
        }

        private void Convert(object p)
        {
            if (p == null)
            {
                foreach (var i in WarehouseEntries)
                {
                    if (!i.IsLoad)
                        ConvertItem(i);
                }
            }
            else if (p is WarehouseEntry)
            {
                ConvertItem(p as WarehouseEntry);
            }
        }


        private void ConvertItem(WarehouseEntry we)
        {
            if (we.IsLoad)
            {
                we.Amount *= Math.Max(we.Item.Load, 1);
                we.Item.Unit = unitDict[we.ItemId];
            }
            else
            {
                we.Amount /= Math.Max(we.Item.Load, 1);
                unitDict[we.ItemId] = we.Item.Unit;
                we.Item.Unit = "箱";
            }
            we.IsLoad = !we.IsLoad;
        }
    }
}
