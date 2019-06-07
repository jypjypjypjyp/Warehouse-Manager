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
    public class ItemViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Item> Items { get; set; }
        #endregion

        #region Commands
        public ICommand UpdateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        private readonly ItemView owner;
        public ItemViewModel(ItemView owner)
        {
            this.owner = owner;
            Items = new ObservableCollection<Item>();
            UpdateCommand = DelegateCommand.Create(Update);
            ResetCommand = DelegateCommand.Create(o => InitAsync());
            DeleteCommand = DelegateCommand.Create(Delete);
        }

        public async void InitAsync()
        {
            Items.Clear();
            IEnumerable<Item> allItems = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allItems = db.Items.AsEnumerable();
            });
            foreach (var i in allItems)
            {
                if (i.Available)
                    Items.Add(i);
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
                foreach (var i in Items)
                {
                    if (i.Id != 0)
                    {
                        db.Items.Update(db, i);
                    }
                    else
                    {
                        i.Available = true;
                        db.Items.Add(i);
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
            new InfoDialog("您确认删除此商品吗？原有的记录不会改变，但是不能再进行交易。", true)
            {
                Ok = () => Task.Run(() =>
                {
                    using var db = MyDbContext.Instance;
                    var i = Items.Where(a => a.Id == id).FirstOrDefault();
                    int index = Items.IndexOf(i);
                    i.Available = false;
                    db.Items.Update(db, i);
                    db.SaveChanges();
                    owner.Dispatcher.Invoke(() =>
                    {
                        Items.RemoveAt(index);
                        MainWindowViewModel.Instance.StatusText = "删除成功！";
                    });
                })
            }.Show();
        }

    }
}
