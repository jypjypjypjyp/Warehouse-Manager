using MahApps.Metro.Controls;
using MyWMS.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace MyWMS.Views
{
    public partial class WarehousePrintDialog : MetroWindow
    {
        private System.Windows.Xps.Packaging.XpsDocument xpsDoc;
        private readonly WarehouseDetail owner;
        public WarehousePrintDialog(WarehouseDetail owner)
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
                    Environment.CurrentDirectory + "\\库存模板.xlsx" : Properties.Settings.Default.TamplatePath;
                    var tempFilePath = Environment.CurrentDirectory + "\\temp.xlsx";
                    File.Copy(excelFilePath, Environment.CurrentDirectory + "\\temp.xlsx", true);
                    if (write) WriteWarehouseToExcel(tempFilePath);
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

        private void WriteWarehouseToExcel(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook book = null;
            try
            {
                // Prepare for Data
                string id = owner.VM.Id.ToString();
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                var table = new object[owner.VM.WarehouseEntries.Count, 4];
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    var t = owner.VM.WarehouseEntries[i];
                    table[i, 0] = t.Item.Name;
                    table[i, 1] = t.Item.Specification ?? "";
                    table[i, 2] = t.Item.Unit ?? "";
                    table[i, 3] = t.Amount;
                }
                // Write into an Excel file
                excelApp = new Excel.Application();
                book = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet workSheet = book.Worksheets[1];
                if (Properties.Settings.Default.WIdPosR > 0 && Properties.Settings.Default.WIdPosC > 0)
                    workSheet.Cells[Properties.Settings.Default.WIdPosR, ToColumn(Properties.Settings.Default.WIdPosC)] = id;
                if (Properties.Settings.Default.WDatePosR > 0 && Properties.Settings.Default.WDatePosC > 0)
                    workSheet.Cells[Properties.Settings.Default.DatePosR, ToColumn(Properties.Settings.Default.DatePosC)] = time;
                if (Properties.Settings.Default.WTablePosR > 0 && Properties.Settings.Default.WTablePosC > 0)
                {
                    var range = workSheet.Range[
                    ToColumn(Properties.Settings.Default.WTablePosC) + Properties.Settings.Default.WTablePosR.ToString(),
                    ToColumn(Properties.Settings.Default.WTablePosC + table.GetLength(1) - 1) + (Properties.Settings.Default.WTablePosR + table.GetLength(0) - 1).ToString()];
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
