using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;

namespace MyWMS.ViewModels
{
    public class SalemanViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Salesman> Salesmen { get; set; }
        #endregion

        #region Commands
        public ICommand UpdateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        private SalemanView owner;
        public SalemanViewModel(SalemanView owner)
        {
            this.owner = owner;
            Salesmen = new ObservableCollection<Salesman>();
            UpdateCommand = DelegateCommand.Create(Update);
            ResetCommand = DelegateCommand.Create(o => InitAsync());
            DeleteCommand = DelegateCommand.Create(Delete);
        }

        public async void InitAsync()
        {
            Salesmen.Clear();
            IEnumerable<Salesman> allSalemen = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allSalemen = db.Salesmen.AsEnumerable();
            });
            foreach (var i in allSalemen)
            {
                if (i.Available)
                    Salesmen.Add(i); ;
                maxId = Math.Max(maxId, i.Id);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }
    }
}
