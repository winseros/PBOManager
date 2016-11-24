using System.Text;

namespace PboManager.Components.Converters
{
    public static class TextToWindowTitle
    {
        private const string ApplicationName = "PboManager 2.0";

        public static string Convert(string text)
        {
            if (string.IsNullOrEmpty(text))
                return TextToWindowTitle.ApplicationName;

            var builder = new StringBuilder();
            builder.Append(text);
            builder.Append(" - ");
            builder.Append(TextToWindowTitle.ApplicationName);
            return builder.ToString();
        }
    }
}
