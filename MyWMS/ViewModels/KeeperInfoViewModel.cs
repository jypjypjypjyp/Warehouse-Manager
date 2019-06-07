using MyWMS.Helpers;
using MyWMS.Views;
using System.Threading.Tasks;

namespace MyWMS.ViewModels
{
    public class KeeperInfoViewModel : ViewModelBase
    {
        #region Properties
        private int _Id;
        public int Id
        {
            get => _Id;
            set => SetProperty(ref _Id, value);
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                var t = MainWindowViewModel.Instance.CurKeeper;
                t.Name = value;
                MainWindowViewModel.Instance.CurKeeper = t;
                Update();
                SetProperty(ref _Name, value);
            }
        }
        private string _Contact;
        public string Contact
        {
            get => _Contact;
            set
            {
                var t = MainWindowViewModel.Instance.CurKeeper;
                t.Contact = value;
                MainWindowViewModel.Instance.CurKeeper = t;
                Update();
                SetProperty(ref _Contact, value);
            }
        }
        #endregion

        #region Commands

        #endregion

        private readonly KeeperInfoView owner;
        public KeeperInfoViewModel(KeeperInfoView owner)
        {
            this.owner = owner;
        }

        public void Init()
        {
            Id = MainWindowViewModel.Instance.CurKeeper.Id;
            SetProperty(ref _Name, MainWindowViewModel.Instance.CurKeeper.Name, "Name");
            SetProperty(ref _Contact, MainWindowViewModel.Instance.CurKeeper.Contact, "Contact");
        }

        private void Update()
        {
            Task.Run(() =>
            {
                using var db = MyDbContext.Instance;
                db.Keepers.Update(db, MainWindowViewModel.Instance.CurKeeper);
                db.Entry(MainWindowViewModel.Instance.CurKeeper)
                .Property(x => x.Password).IsModified = false;
                db.SaveChanges();
            });
        }

    }
}
