using System;
using System.Globalization;
using PboManager.Converters;

namespace PboManager.Components.MainWindow
{
    public class WindowTitleConverter : ConverterBase
    {
        public static readonly WindowTitleConverter NSTANCE = new WindowTitleConverter();

        private WindowTitleConverter()
        {
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var file = value as PboFileModel;
            string result = TextToWindowTitle.Convert(file?.Name);
            return result;
        }
    }
}
