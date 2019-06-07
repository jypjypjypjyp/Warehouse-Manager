using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
namespace MyWMS.Helpers
{
    public class OfficeToXps
    {
        #region Properties & Constants

        private static List<string> excelExtensions = new List<string>
        {
            ".xls",
            ".xlsx"
        };

        #endregion

        public static int TempNum = 0;

        #region Public Methods
        public static OfficeToXpsConversionResult ConvertToXps(string sourceFilePath, ref string resultFilePath)
        {
            var result = new OfficeToXpsConversionResult(ConversionResult.UnexpectedError);

            // Check to see if it's a valid file
            if (!IsValidFilePath(sourceFilePath))
            {
                result.Result = ConversionResult.InvalidFilePath;
                result.ResultText = sourceFilePath;
                return result;
            }



            var ext = Path.GetExtension(sourceFilePath).ToLower();

            // Check to see if it's in our list of convertable extensions
            if (!IsConvertableFilePath(sourceFilePath))
            {
                result.Result = ConversionResult.InvalidFileExtension;
                result.ResultText = ext;
                return result;
            }

            // Convert if Excel
            if (excelExtensions.Contains(ext))
            {
                return ConvertFromExcel(sourceFilePath, ref resultFilePath);
            }

            return result;
        }
        #endregion

        #region Private Methods
        public static bool IsValidFilePath(string sourceFilePath)
        {
            if (string.IsNullOrEmpty(sourceFilePath))
                return false;

            try
            {
                return File.Exists(sourceFilePath);
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool IsConvertableFilePath(string sourceFilePath)
        {
            var ext = Path.GetExtension(sourceFilePath).ToLower();

            return IsConvertableExtension(ext);
        }
        public static bool IsConvertableExtension(string extension)
        {
            return excelExtensions.Contains(extension);
        }

        private static string GetTempXpsFilePath()
        {
            return Path.ChangeExtension(Path.GetTempFileName(), ".xps");
        }

        private static OfficeToXpsConversionResult ConvertFromExcel(string sourceFilePath, ref string resultFilePath)
        {
            string pSourceDocPath = sourceFilePath;

            string pExportFilePath = string.IsNullOrWhiteSpace(resultFilePath) ? GetTempXpsFilePath() : resultFilePath;

            try
            {
                var pExportFormat = Excel.XlFixedFormatType.xlTypeXPS;
                var pExportQuality = Excel.XlFixedFormatQuality.xlQualityStandard;
                var pOpenAfterPublish = false;
                var pIncludeDocProps = true;
                var pIgnorePrintAreas = true;


                Excel.Application excelApplication = null;
                Excel.Workbook excelWorkbook = null;

                try
                {
                    excelApplication = new Excel.Application();
                }
                catch (Exception exc)
                {
                    return new OfficeToXpsConversionResult(ConversionResult.ErrorUnableToInitializeOfficeApp, "Excel", exc);
                }

                try
                {
                    try
                    {
                        excelWorkbook = excelApplication.Workbooks.Open(pSourceDocPath);
                    }
                    catch (Exception exc)
                    {
                        return new OfficeToXpsConversionResult(ConversionResult.ErrorUnableToOpenOfficeFile, exc.Message, exc);
                    }

                    if (excelWorkbook != null)
                    {
                        try
                        {
                            excelWorkbook.ExportAsFixedFormat(
                                                pExportFormat,
                                                pExportFilePath,
                                                pExportQuality,
                                                pIncludeDocProps,
                                                pIgnorePrintAreas,

                                                OpenAfterPublish: pOpenAfterPublish
                                            );
                        }
                        catch (Exception exc)
                        {
                            return new OfficeToXpsConversionResult(ConversionResult.ErrorUnableToExportToXps, "Excel", exc);
                        }
                    }
                    else
                    {
                        return new OfficeToXpsConversionResult(ConversionResult.ErrorUnableToOpenOfficeFile);
                    }
                }
                finally
                {
                    // Close and release the Document object.
                    if (excelWorkbook != null)
                    {
                        excelWorkbook.Close();
                        excelWorkbook = null;
                    }

                    // Quit Word and release the ApplicationClass object.
                    if (excelApplication != null)
                    {
                        excelApplication.Quit();
                        excelApplication = null;
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception exc)
            {
                return new OfficeToXpsConversionResult(ConversionResult.ErrorUnableToAccessOfficeInterop, "Excel", exc);
            }

            resultFilePath = pExportFilePath;

            return new OfficeToXpsConversionResult(ConversionResult.OK, pExportFilePath);
        }
        #endregion
    }

    public class OfficeToXpsConversionResult
    {
        #region Properties
        public ConversionResult Result { get; set; }
        public string ResultText { get; set; }
        public Exception ResultError { get; set; }
        #endregion

        #region Constructors
        public OfficeToXpsConversionResult()
        {
            Result = ConversionResult.UnexpectedError;
            ResultText = string.Empty;
        }
        public OfficeToXpsConversionResult(ConversionResult result)
            : this()
        {
            Result = result;
        }
        public OfficeToXpsConversionResult(ConversionResult result, string resultText)
            : this(result)
        {
            ResultText = resultText;
        }
        public OfficeToXpsConversionResult(ConversionResult result, string resultText, Exception exc)
            : this(result, resultText)
        {
            ResultError = exc;
        }
        #endregion
    }

    public enum ConversionResult
    {
        OK = 0,
        InvalidFilePath = 1,
        InvalidFileExtension = 2,
        UnexpectedError = 3,
        ErrorUnableToInitializeOfficeApp = 4,
        ErrorUnableToOpenOfficeFile = 5,
        ErrorUnableToAccessOfficeInterop = 6,
        ErrorUnableToExportToXps = 7
    }
}
