using MyWMS.Helpers;
using MyWMS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using MyWMS.Views;
using System.Threading;
using System;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class DealViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Salesman> Salesmen { get; set; }
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

        public ManualResetEvent InitFinishedEvent;
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
        public ICommand AddCommand { get; set; }
        public ICommand ToDetailCommand { get; set; }
        #region Commands

        #endregion
        private DealView owner;
        public DealViewModel(DealView owner)
        {
            this.owner = owner;
            Salesmen = new ObservableCollection<Salesman>();
            Warehouses = new ObservableCollection<Warehouse>();
            Deals = new ObservableCollection<Deal>();
            _FromDateTime = DateTime.Now;
            _ToDateTime = DateTime.MinValue;
            AddCommand = DelegateCommand.Create(Add);
            ToDetailCommand = DelegateCommand.Create(ToDetail);
            InitFinishedEvent = new ManualResetEvent(false);
        }

        public async void InitAsync()
        {
            Salesmen.Clear();
            Warehouses.Clear();
            IEnumerable<Warehouse> allWarehouse = null;
            IEnumerable<Salesman> allSalesman = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allWarehouse = db.Warehouses.AsEnumerable();
                allSalesman = db.Salesmen.AsEnumerable();
            });
            foreach (var i in allWarehouse)
            {
                if(i.Available)
                    Warehouses.Add(i);
            }
            foreach (var i in allSalesman)
            {
                if(i.Available)
                    Salesmen.Add(i);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
            InitFinishedEvent.Set();
        }

        private bool isUpdating = false;
        private async void UpdateFilter()
        {
            if (isUpdating) return;
            else isUpdating = true;
            Deals.Clear();
            using var db = MyDbContext.Instance;
            IQueryable<Deal> query = null;
            await Task.Run(() =>
            {
                query = db.Deals.Where(a => a.Time <= FromDateTime && a.Time >= ToDateTime);
                if (SelectedWarehouse != null)
                {
                    query = query.Where(a => a.WarehouseId == SelectedWarehouse.Id);
                }
                if (SelectedSalesman != null)
                {
                    query = query.Where(a => a.SalesmanId == SelectedSalesman.Id);
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
    }
}
