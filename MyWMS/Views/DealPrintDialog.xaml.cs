using MahApps.Metro.Controls;
using MyWMS.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace MyWMS.Views
{
    public partial class DealPrintDialog : MetroWindow
    {
        
        private System.Windows.Xps.Packaging.XpsDocument xpsDoc;
        private readonly DealDetail owner;
        public DealPrintDialog(DealDetail owner)
        {
            this.owner = owner;
            InitializeComponent();
            Preview(false);
        }

        private void Preview(bool write)
        {
            var progressDialog = new ProgressDialog(false);
            progressDialog.Show();
            Previewer.Document = null;
            try
            {
                Task.Run(() =>
                {
                    var xpsFilePath = Environment.CurrentDirectory + $"\\{OfficeToXps.TempNum++}.xps";
                    var excelFilePath = (Properties.Settings.Default.TamplatePath == "") ?
                    Environment.CurrentDirectory + "\\订单模板.xlsx" : Properties.Settings.Default.TamplatePath;
                    var tempFilePath = Environment.CurrentDirectory + "\\temp.xlsx";
                    File.Copy(excelFilePath, Environment.CurrentDirectory + "\\temp.xlsx", true);
                    if (write) WriteDealToExcel(tempFilePath);
                    var convertResults = OfficeToXps.ConvertToXps(tempFilePath, ref xpsFilePath);
                    switch (convertResults.Result)
                    {
                        case ConversionResult.OK:
                            xpsDoc = new System.Windows.Xps.Packaging.XpsDocument(xpsFilePath, FileAccess.ReadWrite);
                            Dispatcher.Invoke(() =>
                                {
                                    Previewer.Document = xpsDoc.GetFixedDocumentSequence();
                                });

                            break;
                        default:
                            throw new Exception();
                    }
                });
            }
            catch
            {
                new InfoDialog("请安装Microsoft Office！", false).Show();
            }
            finally
            {
                progressDialog.Close();
            }
        }

        private void WriteDealToExcel(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook book = null;
            try
            {
                // Prepare for Data
                using var db = MyDbContext.Instance;
                var deal = owner.VM.Deal;
                string time = deal.Time.ToString("yyyy-MM-dd");
                string num = deal.GetNumber();
                string keeperName = db.Keepers.Find(deal.KeeperId).Name;
                string keeperContact = db.Keepers.Find(deal.KeeperId).Contact;
                string customer = db.Customers.Find(deal.CustomerId).Name;
                var table = new object[owner.VM.DealEntries.Count, 8];
                double sum = 0;
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    var t = owner.VM.DealEntries[i];
                    table[i, 0] = t.Item.Name;
                    table[i, 2] = t.Item.Specification ?? "";
                    table[i, 3] = t.Item.Unit ?? "";
                    table[i, 4] = t.Amount.ToString("F");
                    table[i, 5] = t.Prize.ToString("C");
                    table[i, 6] = (t.Amount * t.Prize).ToString("C");
                    table[i, 7] = t.Note;
                    sum += t.Amount * t.Prize;
                }
                string chineseSum = "合计金额（大写）    " + (string)new CurrencyToChineseConverter().Convert(sum, null, null, null);
                string sumStr = sum.ToString("C");
                // Write into an Excel file
                excelApp = new Excel.Application();
                book = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet workSheet = book.Worksheets[1];
                if (Properties.Settings.Default.DatePosR > 0 && Properties.Settings.Default.DatePosC > 0)
                    workSheet.Cells[Properties.Settings.Default.DatePosR, ToColumn(Properties.Settings.Default.DatePosC)] = time;
                if (Properties.Settings.Default.NumPosR > 0 && Properties.Settings.Default.NumPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.NumPosR, ToColumn(Properties.Settings.Default.NumPosC)] = num;
                if (Properties.Settings.Default.KeeperPosR > 0 && Properties.Settings.Default.KeeperPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.KeeperPosR, ToColumn(Properties.Settings.Default.KeeperPosC)] = keeperName;
                if (Properties.Settings.Default.KeeperContactPosR > 0 && Properties.Settings.Default.KeeperContactPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.KeeperContactPosR, ToColumn(Properties.Settings.Default.KeeperContactPosC)] = keeperContact;
                if (Properties.Settings.Default.CustomerPosR > 0 && Properties.Settings.Default.CustomerPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.CustomerPosR, ToColumn(Properties.Settings.Default.CustomerPosC)] = customer;
                if (Properties.Settings.Default.ChineseSumPosR > 0 && Properties.Settings.Default.ChineseSumPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.ChineseSumPosR, ToColumn(Properties.Settings.Default.ChineseSumPosC)] = chineseSum;
                if (Properties.Settings.Default.SumPosR > 0 && Properties.Settings.Default.SumPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.SumPosR, ToColumn(Properties.Settings.Default.SumPosC)] = sumStr;
                if (Properties.Settings.Default.TablePosR > 0 && Properties.Settings.Default.TablePosC > 0)
                {
                    var range = workSheet.Range[
                    ToColumn(Properties.Settings.Default.TablePosC) + Properties.Settings.Default.TablePosR.ToString(),
                    ToColumn(Properties.Settings.Default.TablePosC + table.GetLength(1) - 1) + (Properties.Settings.Default.TablePosR + table.GetLength(0) - 1).ToString()];
                    range.Value2 = table;
                }
            }
            finally
            {
                book.Close(true);
                excelApp?.Quit();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void OpenTemplate_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "Excel Files(*.xlsx;*.xls)|*.xlsx;*.xls";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Properties.Settings.Default.TamplatePath = dlg.FileName;
                Preview(false);
            }
        }

        private static string ToColumn(int column)
        {
            return char.ToString((char)('@' + column));
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            Preview(true);
        }
    }
}
