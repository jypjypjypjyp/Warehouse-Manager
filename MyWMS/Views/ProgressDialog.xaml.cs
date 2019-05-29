using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace MyWMS.Views
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class ProgressDialog : MetroWindow
    {
        public Action Cancel { get; set; }

        public ProgressDialog(bool showCancel)
        {
            InitializeComponent();
            CancelBtn.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke();
            Close();
        }
    }
}
