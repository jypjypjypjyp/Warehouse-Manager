using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class DealDetailViewModel : ViewModelBase
    {
        #region Properties
        private string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        public Deal Deal;

        private int _Id;
        public int Id
        {
            get => _Id;
            set
            {
                Deal.Id = value;
                SetProperty(ref _Id, value);
            }
        }
        private bool _InOrOut;
        public bool InOrOut
        {
            get => _InOrOut;
            set
            {
                Deal.InOrOut = value;
                SetProperty(ref _InOrOut, value);
            }
        }
        private DateTime _Time;
        public DateTime Time
        {
            get => _Time;
            set
            {
                Deal.Time = value;
                SetProperty(ref _Time, value);
            }
        }
        private Warehouse _Warehouse;
        public Warehouse Warehouse
        {
            get => _Warehouse;
            set
            {
                Deal.WarehouseId = value != null ? value.Id : 0;
                Deal.Warehouse = value;
                SetProperty(ref _Warehouse, value);
            }
        }
        private Salesman _Salesman;
        public Salesman Salesman
        {
            get => _Salesman;
            set
            {
                Deal.SalesmanId = value != null ? value.Id : 0;
                Deal.Salesman = value;
                SetProperty(ref _Salesman, value);
            }
        }
        private Customer _Customer;
        public Customer Customer
        {
            get => _Customer;
            set
            {
                Deal.CustomerId = value != null ? value.Id : 0;
                Deal.Customer = value;
                SetProperty(ref _Customer, value);
            }
        }

        private double _Sum;
        public double Sum
        {
            get => _Sum;
            set
            {
                SetProperty(ref _Sum, value);
            }
        }

        private bool _Editable;
        public bool Editable
        {
            get => _Editable;
            set
            {
                SetProperty(ref _Editable, value);
            }
        }

        private bool _Printable;
        public bool Printable
        {
            get => _Printable;
            set
            {
                SetProperty(ref _Printable, value);
            }
        }
        public ObservableCollection<DealEntry> DealEntries { get; set; }
        #endregion

        #region Command
        public ICommand ConfirmCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion


        private readonly DealDetail owner;
        public DealDetailViewModel(DealDetail owner)
        {
            this.owner = owner;
            Deal = new Deal();
            DealEntries = new ObservableCollection<DealEntry>();
            ConfirmCommand = AsyncCommand.Create(Commit);
            ResetCommand = DelegateCommand.Create(o => InitAsync(Editable, Deal.Id));
            DeleteCommand = DelegateCommand.Create(Delete);
        }

        public async void InitAsync(bool editable, int id)
        {
            DealEntries.Clear();
            Editable = editable;
            Printable = !editable;
            Id = id;
            Deal.KeeperId = MainWindowViewModel.Instance.CurKeeper.Id;
            Sum = 0;
            if (editable)
            {
                Title = "";
                InOrOut = true;
                Time = DateTime.Now;
                Warehouse = null;
                Salesman = null;
                Customer = null;
                return;
            }
            using var db = MyDbContext.Instance;
            Deal deal = null;
            IEnumerable<DealEntry> allDealEntries = null;
            await Task.Run(async () =>
            {
                var t1 = db.Deals.FindAsync(id);
                allDealEntries = db.DealEntries.Include(a => a.Item).Where(a => a.DealId == id).AsEnumerable();
                deal = await t1;
            });
            InOrOut = deal.InOrOut;
            Time = deal.Time;
            Warehouse = owner.Owner.VM.Warehouses.Where(a => a.Id == deal.WarehouseId).FirstOrDefault() ?? new Warehouse() { Id = deal.WarehouseId };
            Salesman = owner.Owner.VM.Salesmen.Where(a => a.Id == deal.SalesmanId).FirstOrDefault() ?? new Salesman() { Id = deal.SalesmanId };
            Customer = owner.Owner.VM.Customers.Where(a => a.Id == deal.CustomerId).FirstOrDefault() ?? new Customer() { Id = deal.CustomerId };
            foreach (var i in allDealEntries)
            {
                DealEntries.Add(i);
                Sum += i.Amount * i.Prize;
            }
            Title = $"编号：{Deal.GetNumber()} 仓库：{Deal.WarehouseId} 销售员ID：{Deal.SalesmanId} 客户：{Deal.CustomerId}";
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }
        public async void InitAsync(int[] id)
        {
            DealEntries.Clear();
            Editable = false;
            Printable = false;
            Sum = 0;
            Title = "";
            InOrOut = true;
            Time = DateTime.Now;
            Warehouse = null;
            Salesman = null;
            Customer = null;

            using var db = MyDbContext.Instance;
            IEnumerable<DealEntry> allDealEntries = null;
            await Task.Run(() =>
            {
                allDealEntries = db.DealEntries.Include(a => a.Item).Where(a => id.Contains(a.DealId)).AsEnumerable();
            });
            foreach (var i in allDealEntries)
            {
                DealEntries.Add(i);
                Sum += i.Amount * i.Prize;
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }

        public void AddEntry(Item item, double amount, double prize, string note)
        {
            DealEntries.Add(new DealEntry()
            {
                Item = item,
                ItemId = item.Id,
                Note = note,
                Prize = prize,
                Amount = amount
            });
            Sum += amount * prize;
            MainWindowViewModel.Instance.StatusText = "添加成功！";
        }

        public async Task Commit(object p, CancellationToken token)
        {
            var progressDialog = new ProgressDialog(false);
            progressDialog.Show();
            try
            {
                var deal = new Deal
                {
                    Id = Id,
                    Time = Time,
                };
                if (p is ValueTuple<int, bool>)
                {
                    deal.WarehouseId = (((int, bool))p).Item1;
                    deal.SalesmanId = 0;
                    deal.CustomerId = 0;
                    deal.InOrOut = (((int, bool))p).Item2;
                }
                else if (p == null)
                {
                    if (Salesman == null || Warehouse == null || Customer == null)
                        throw new Exception();
                    deal.InOrOut = InOrOut;
                    deal.WarehouseId = Warehouse.Id;
                    deal.SalesmanId = Salesman.Id;
                    deal.CustomerId = Customer.Id;
                }

                //Cheack & Update
                using var db = MyDbContext.Instance;
                var dict = new Dictionary<int, double>();
                foreach (var i in DealEntries)
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
                    foreach (var i in dict)
                    {
                        var we = db.WarehouseEntries.Find(deal.WarehouseId, i.Key);
                        if (deal.InOrOut)
                        {
                            if (we == null || (we.Amount -= i.Value) < 0)
                            {
                                throw new Exception(i.Key.ToString());
                            }
                            else
                            {
                                if (we.Amount == 0)
                                    db.WarehouseEntries.Remove(we);
                                else
                                    db.WarehouseEntries.Update(db, we);
                            }
                        }
                        else
                        {
                            if (we == null)
                            {
                                db.WarehouseEntries.Add(new WarehouseEntry()
                                {
                                    WarehouseId = deal.WarehouseId,
                                    ItemId = i.Key,
                                    Amount = i.Value
                                });
                            }
                            else
                            {
                                we.Amount += i.Value;
                                db.WarehouseEntries.Update(db, we);
                            }
                        }
                    }
                    db.Deals.Add(deal);
                    db.SaveChanges();
                    foreach (var i in DealEntries)
                    {
                        i.DealId = deal.Id;
                        i.Item = null;
                        db.DealEntries.Add(i);
                    }
                    if(deal.CustomerId != 0)
                    {
                        Customer.Money += deal.InOrOut ? -Sum : Sum;
                        db.Customers.Update(db, Customer);
                    }
                    db.SaveChanges();
                });
                owner.Owner.CloseDetail();
                owner.Owner.VM.Deals.Add(deal);
                if (p is ValueTuple<int, bool>)
                    new InfoDialog($"{(deal.InOrOut ? "出" : "入")}库成功！", false).Show();
                else
                    new InfoDialog($"客户应{(deal.InOrOut ? "付" : "收")}{Sum.ToString("C")}", false).Show();
                MainWindowViewModel.Instance.StatusText = "更新成功！";
            }
            catch (Exception e)
            {
                new InfoDialog($"库存不足！商品编号：{e.Message}", true)
                {
                    Cancel = () => InitAsync(Editable, Id)
                }.Show();
            }
            finally
            {
                progressDialog.Close();
            }
        }

        private async void Delete(object p)
        {
            if (_Editable == true)
                return;
            var progressDialog = new ProgressDialog(false);
            progressDialog.Show();
            try
            {
                using var db = MyDbContext.Instance;
                int factor = InOrOut ? 1 : -1;
                await Task.Run(() =>
                {
                    foreach (var i in DealEntries)
                    {
                        var we = db.WarehouseEntries.Find(Warehouse.Id, i.ItemId);
                        if (we == null)
                        {
                            we = new WarehouseEntry
                            {
                                WarehouseId = Warehouse.Id,
                                ItemId = i.ItemId,
                                Amount = factor * i.Amount
                            };
                            if (we.Amount < 0)
                                throw new Exception();
                            else if (we.Amount > 0)
                                db.WarehouseEntries.Add(we);
                        }
                        else
                        {
                            we.Amount += factor * i.Amount;
                            if (we.Amount < 0)
                                throw new Exception();
                            else if (we.Amount == 0)
                                db.WarehouseEntries.Remove(we);
                            else
                                db.WarehouseEntries.Update(db, we);
                        }
                    }
                    db.Deals.Attach(Deal);
                    db.Deals.Remove(Deal);
                    db.SaveChanges();
                });
                owner.Owner.VM.UpdateFilter();
                MainWindowViewModel.Instance.StatusText = "删除成功！";
                owner.Owner.CloseDetail();
            }
            catch
            {
                new InfoDialog("库存错误，请保证充足库存再删除改订单。", false).Show();
                MainWindowViewModel.Instance.StatusText = "数据错误！";
            }
            finally
            {
                progressDialog.Close();
            }
        }
    }
}
