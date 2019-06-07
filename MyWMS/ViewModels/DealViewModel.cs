using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class DealViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Salesman> Salesmen { get; set; }
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Warehouse> Warehouses { get; set; }
        public ObservableCollection<Deal> Deals { get; set; }

        private Salesman _SelectedSalesman;
        public Salesman SelectedSalesman
        {
            get => _SelectedSalesman;
            set
            {
                SetProperty(ref _SelectedSalesman, value);
                UpdateFilter();
            }
        }
        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                SetProperty(ref _SelectedCustomer, value);
                UpdateFilter();
            }
        }

        private Warehouse _SelectedWarehouse;
        public Warehouse SelectedWarehouse
        {
            get => _SelectedWarehouse;
            set
            {
                SetProperty(ref _SelectedWarehouse, value);
                UpdateFilter();
            }
        }

        private DateTime _FromDateTime;
        public DateTime FromDateTime
        {
            get => _FromDateTime;
            set
            {
                SetProperty(ref _FromDateTime, value);
                UpdateFilter();
            }
        }

        private DateTime _ToDateTime;
        public DateTime ToDateTime
        {
            get => _ToDateTime;
            set
            {
                SetProperty(ref _ToDateTime, value);
                UpdateFilter();
            }
        }

        #endregion

        #region Commands
        public ICommand AddCommand { get; set; }
        public ICommand ToDetailCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        public ICommand ToSumCommand { get; set; }
        #endregion

        public ManualResetEvent InitFinishedEvent;
        private readonly DealView owner;
        public DealViewModel(DealView owner)
        {
            this.owner = owner;
            Salesmen = new ObservableCollection<Salesman>();
            Customers = new ObservableCollection<Customer>();
            Warehouses = new ObservableCollection<Warehouse>();
            Deals = new ObservableCollection<Deal>();
            SetProperty(ref _FromDateTime, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0), "FromDateTime");
            SetProperty(ref _ToDateTime, DateTime.MinValue, "ToDateTime");
            AddCommand = DelegateCommand.Create(Add);
            ToDetailCommand = DelegateCommand.Create(ToDetail);
            ClearFilterCommand = DelegateCommand.Create(ClearFilter);
            ToSumCommand = DelegateCommand.Create(ToSum);
            InitFinishedEvent = new ManualResetEvent(false);
        }

        public async void InitAsync()
        {
            Salesmen.Clear();
            Warehouses.Clear();
            IEnumerable<Warehouse> allWarehouses = null;
            IEnumerable<Salesman> allSalesmen = null;
            IEnumerable<Customer> allCustomers = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allWarehouses = db.Warehouses.AsEnumerable();
                allSalesmen = db.Salesmen.AsEnumerable();
                allCustomers = db.Customers.AsEnumerable();
            });
            foreach (var i in allWarehouses)
            {
                if (i.Available)
                    Warehouses.Add(i);
            }
            foreach (var i in allSalesmen)
            {
                if (i.Available)
                    Salesmen.Add(i);
            }
            foreach (var i in allCustomers)
            {
                if (i.Available)
                    Customers.Add(i);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
            InitFinishedEvent.Set();
        }

        private bool isUpdating = false;
        public async void UpdateFilter()
        {
            if (isUpdating) return;
            else isUpdating = true;
            Deals.Clear();
            using var db = MyDbContext.Instance;
            IQueryable<Deal> query = null;
            await Task.Run(() =>
            {
                var t = FromDateTime.AddDays(1);
                query = db.Deals.Where(a => a.Time <= t && a.Time >= ToDateTime);
                if (SelectedWarehouse != null)
                {
                    query = query.Where(a => a.WarehouseId == SelectedWarehouse.Id);
                }
                if (SelectedSalesman != null)
                {
                    query = query.Where(a => a.SalesmanId == SelectedSalesman.Id);
                }
                if (SelectedCustomer != null)
                {
                    query = query.Where(a => a.CustomerId == SelectedCustomer.Id);
                }
            });
            foreach (var i in query)
            {
                Deals.Add(i);
            }
            isUpdating = false;
        }

        private void Add(object p)
        {
            owner.ToDetail((true, 0));
        }

        private void ToDetail(object p)
        {
            owner.ToDetail((false, (int)p));
        }

        private void ToSum(object p)
        {
            owner.ToSum(Deals.Select(a => a.Id).ToArray());
        }

        private void ClearFilter(object p)
        {
            SetProperty(ref _SelectedSalesman, null, "SelectedSalesman");
            SetProperty(ref _SelectedCustomer, null, "SelectedCustomer");
            SetProperty(ref _SelectedWarehouse, null, "SelectedWarehouse");
            SetProperty(ref _FromDateTime, DateTime.Now, "FromDateTime");
            SetProperty(ref _ToDateTime, DateTime.MinValue, "ToDateTime");
            UpdateFilter();
        }
    }
}
