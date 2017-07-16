using System;
using System.Globalization;

namespace PboManager.Components.MainWindow
{
    public class WindowTitleConverter : ConverterBase
    {
        public static readonly WindowTitleConverter NSTANCE = new WindowTitleConverter();
        private const string APP_NAME = "PBOManager";

        private WindowTitleConverter()
        {
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result;
            if (value is PboFileModel file)
                result = string.Concat(file.Name, " - ", WindowTitleConverter.APP_NAME);
            else
                result = WindowTitleConverter.APP_NAME;
            return result;
        }
    }
}
