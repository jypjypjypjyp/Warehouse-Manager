using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace MyWMS.Views
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : MetroWindow
    {
        public Action Ok { get; set; }
        public Action Cancel { get; set; }

        public InfoDialog(string message, bool showCancel)
        {
            InitializeComponent();
            Message.Text = message;
            CancelBtn.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Ok?.Invoke();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke();
            Close();
        }
    }
}
