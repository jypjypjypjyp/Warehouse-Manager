using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyWMS.Helpers;
using MyWMS.Models;
using MyWMS.Views;

namespace MyWMS.ViewModels
{
    public class KeeperViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Keeper> Keepers { get; set; }
        #endregion

        #region Commands

        #endregion
        private readonly KeeperView owner;
        public KeeperViewModel(KeeperView owner)
        {
            this.owner = owner;
            Keepers = new ObservableCollection<Keeper>();
        }

        public async void InitAsync()
        {
            Keepers.Clear();
            IEnumerable<Keeper> allKeepers = null;
            using var db = MyDbContext.Instance;
            await Task.Run(() =>
            {
                allKeepers = db.Keepers.AsEnumerable();
            });
            foreach (var i in allKeepers)
            {
                if (i.Available)
                    Keepers.Add(i);
            }
            MainWindowViewModel.Instance.StatusText = "载入成功！";
        }

        public void AddKeeper(string name, string contact)
        {
            using var db = MyDbContext.Instance;
            db.Keepers.Add(new Keeper()
            {
                Name = name,
                Contact = contact,
                Password = "123",
                Available = true
            });
            db.SaveChanges();
            InitAsync();
        }
    }
}
