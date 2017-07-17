namespace PboManager.Converters
{
    public static class TextToWindowTitle
    {
        private const string APPLICATION_NAME = "PboManager 2.0";

        public static string Convert(string text)
        {
            if (string.IsNullOrEmpty(text))
                return TextToWindowTitle.APPLICATION_NAME;

            string title = string.Concat(text, " - ", TextToWindowTitle.APPLICATION_NAME);
            return title;
        }
    }
}
