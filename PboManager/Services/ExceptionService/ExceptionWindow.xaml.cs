using System;
using System.Threading.Tasks;
using System.Windows;
using PboManager.Converters;
using Util;

namespace PboManager.Services.ExceptionService
{    
    public partial class ExceptionWindow : Window
    {
        private const string TEXT_DETAILS_OPEN = "Show Details";
        private const string TEXT_DETAILS_CLOSE = "Hide Details";
        private const string TEXT_COPY = "Copy";
        private const string TEXT_COPIED = "Copied..";

        public ExceptionWindow(string message, Exception ex)
        {
            Assert.NotNull(message, nameof(message));
            Assert.NotNull(ex, nameof(ex));
            this.InitializeComponent();

            this.Title = TextToWindowTitle.Convert("Error");
            this.BtnDetails.Content = ExceptionWindow.TEXT_DETAILS_OPEN;
            this.BtnCopy.Content = ExceptionWindow.TEXT_COPY;

            this.TxtMessage.Text = message;
            this.TxtStacktrace.Text = string.Concat(ex.Message, Environment.NewLine, ex.StackTrace);

            this.BtnClose.Click += (sender, args) => this.Close();
            this.BtnCopy.Click += this.BtnCopyOnClick;
            this.BtnDetails.Click += this.BtnDetailsOnClick;
        }        

        private void BtnDetailsOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            bool isExpanded = this.BtnCopy.Visibility == Visibility.Visible;
            if (isExpanded)
            {
                this.Height = 200;
                this.BtnCopy.Visibility = Visibility.Collapsed;
                this.TxtStacktrace.Visibility = Visibility.Collapsed;
                this.BtnDetails.Content = ExceptionWindow.TEXT_DETAILS_OPEN;
            }
            else
            {
                this.Height = 400;
                this.BtnCopy.Visibility = Visibility.Visible;
                this.TxtStacktrace.Visibility = Visibility.Visible;
                this.BtnDetails.Content = ExceptionWindow.TEXT_DETAILS_CLOSE;
            }
        }

        private void BtnCopyOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Clipboard.SetText(this.TxtStacktrace.Text);

            if (string.Equals(this.BtnCopy.Content, ExceptionWindow.TEXT_COPY))
            {
                this.BtnCopy.Content = ExceptionWindow.TEXT_COPIED;
                TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Delay(1000).ContinueWith(task => this.BtnCopy.Content = ExceptionWindow.TEXT_COPY, scheduler);
            }            
        }
    }
}
