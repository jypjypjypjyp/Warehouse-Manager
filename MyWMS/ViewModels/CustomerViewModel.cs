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
    public class CustomerViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Customer> Customers { get; set; }
        #endregion

        #region Commands
        public ICommand UpdateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ToDealCommand { get; set; }
        #endregion

        private readonly CustomerView owner;
        public CustomerViewModel(CustomerView owner)
        {
            this.owner = owner;
            Customers = new ObservableCollection<Customer>();
            UpdateCommand = DelegateCommand.Create(Update);
            ResetCommand = DelegateCommand.Create(o => InitAsync());
            DeleteCommand = DelegateCommand.Create(Delete);
            ToDealCommand = DelegateCommand.Create(owner.ToDeal);
        }
        
        public async void InitAsync()
        {
            Customers.Clear();
            IEnumerable<Customer> allCustomers = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allCustomers = db.Customers.AsEnumerable();
            });
            foreach (var i in allCustomers)
            {
                if (i.Available)
                    Customers.Add(i);
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
                foreach (var i in Customers)
                {
                    if (i.Id != 0)
                    {
                        db.Customers.Update(db, i);
                    }
                    else
                    {
                        i.Available = true;
                        db.Customers.Add(i);
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
            new InfoDialog("您确认删除此客户吗？原有的记录不会改变，但是不可再用。", true)
            {
                Ok = () => Task.Run(() =>
                {
                    using var db = MyDbContext.Instance;
                    var i = Customers.Where(a => a.Id == id).FirstOrDefault();
                    int index = Customers.IndexOf(i);
                    i.Available = false;
                    db.Customers.Update(db, i);
                    db.SaveChanges();
                    owner.Dispatcher.Invoke(() =>
                    {
                        Customers.RemoveAt(index);
                        MainWindowViewModel.Instance.StatusText = "删除成功！";
                    });
                })
            }.Show();
        }
    }
}
