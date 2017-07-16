using System;
using System.Globalization;
using System.Windows.Data;

namespace PboManager.Components
{
    public abstract class ConverterBase : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}