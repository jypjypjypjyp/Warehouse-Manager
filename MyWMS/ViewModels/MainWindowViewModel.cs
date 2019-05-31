using MyWMS.Helpers;
using MyWMS.Models;

namespace MyWMS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Singleton
        private static MainWindowViewModel _Instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MainWindowViewModel();
                }
                return _Instance;
            }
        }
        public MainWindow Owner { get; set; }
        public MainWindowViewModel() { }
        #endregion

        #region Properties

        private Keeper _CurKeeper;
        public Keeper CurKeeper
        {
            get => _CurKeeper;
            set
            {
                SetProperty(ref _CurKeeper, value);
                Owner.SetName(_CurKeeper.Name);
            }
        }

        private string _StatusText;
        public string StatusText
        {
            get => _StatusText;
            set => SetProperty(ref _StatusText, "Status: " + value);
        }
        #endregion
    }
}
