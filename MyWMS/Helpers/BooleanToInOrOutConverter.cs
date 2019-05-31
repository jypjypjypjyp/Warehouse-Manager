using System;
using System.Globalization;
using System.Windows.Data;

namespace MyWMS.Helpers
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToInOrOutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "出库" : "入库";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
