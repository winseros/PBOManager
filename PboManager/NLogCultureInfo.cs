using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace PboManager
{
    public class NLogCultureInfo : CultureInfo, ICustomFormatter
    {
        public NLogCultureInfo()
            : base("", false)
        {
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var str = arg as string;
            if (str == null)
            {
                var enumerable = arg as IEnumerable;
                if (enumerable != null)
                {
                    str = FormatEnumerable(enumerable);
                }
                else
                {
                    var formattable = arg as IFormattable;
                    str = formattable?.ToString(format, this) ?? arg.ToString();
                }
            }

            return str;
        }

        private static string FormatEnumerable(IEnumerable obj)
        {
            var sb = new StringBuilder();
            sb.Append("[");

            var first = true;
            foreach (object element in obj)
            {
                if (!first) sb.Append(",");

                if (element is string)
                    sb.Append("\"");

                sb.Append(element);

                if (element is string)
                    sb.Append("\"");

                first = false;
            }

            sb.Append("]");

            return sb.ToString();
        }

        public override object GetFormat(Type formatType)
        {
            object result = formatType == typeof(ICustomFormatter) ? this : null;
            return result;
        }
    }
}