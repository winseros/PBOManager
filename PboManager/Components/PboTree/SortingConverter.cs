using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace PboManager.Components.PboTree
{
    public class SortingConverter : ConverterBase
    {
        private static readonly IComparer COMPARER = new PboNodeComparer();
        public static readonly SortingConverter INSTANCE = new SortingConverter();

        private SortingConverter()
        {
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var view = (ListCollectionView)CollectionViewSource.GetDefaultView(value);
            view.CustomSort = SortingConverter.COMPARER;
            return view;
        }
    }
}