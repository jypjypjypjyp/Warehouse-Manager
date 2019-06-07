using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWMS.ViewModels
{
    public class SalemanViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Salesman> Salemen { get; set; }
        #endregion

        #region Commands
        public ICommand UpdateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ToDealCommand { get; set; }
        #endregion

        private readonly SalemanView owner;
        public SalemanViewModel(SalemanView owner)
        {
            this.owner = owner;
            Salemen = new ObservableCollection<Salesman>();
            UpdateCommand = DelegateCommand.Create(Update);
            ResetCommand = DelegateCommand.Create(o => InitAsync());
            DeleteCommand = DelegateCommand.Create(Delete);
            ToDealCommand = DelegateCommand.Create(owner.ToDeal);
        }

        public async void InitAsync()
        {
            Salemen.Clear();
            IEnumerable<Salesman> allSalemen = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allSalemen = db.Salesmen.AsEnumerable();
            });
            foreach (var i in allSalemen)
            {
                if (i.Available)
                    Salemen.Add(i);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }

        public async void Update(object p)
        {
            var progressDialog = new ProgressDialog(false);
            progressDialog.Show();
            await Task.Run(() =>
            {
                using var db = MyDbContext.Instance;
                foreach (var i in Salemen)
                {
                    if (i.Id != 0)
                    {
                        db.Salesmen.Update(db, i);
                    }
                    else
                    {
                        i.Available = true;
                        db.Salesmen.Add(i);
                    }
                }
                db.SaveChanges();
            });
            progressDialog.Close();
            MainWindowViewModel.Instance.StatusText = "更新成功！";
        }

        public void Delete(object p)
        {
            if (p == null) return;
            int id = (int)p;
            new InfoDialog("您确认删除此人员吗？原有的记录不会改变，但是不可用。", true)
            {
                Ok = () => Task.Run(() =>
                {
                    using var db = MyDbContext.Instance;
                    var i = Salemen.Where(a => a.Id == id).FirstOrDefault();
                    int index = Salemen.IndexOf(i);
                    i.Available = false;
                    db.Salesmen.Update(db, i);
                    db.SaveChanges();
                    owner.Dispatcher.Invoke(() =>
                    {
                        Salemen.RemoveAt(index);
                        MainWindowViewModel.Instance.StatusText = "删除成功！";
                    });
                })
            }.Show();
        }
    }
}
