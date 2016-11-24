using System;
using System.Threading.Tasks;
using System.Windows;
using PboManager.Components.Converters;
using Util;

namespace PboManager.Services.ExceptionService
{    
    public partial class ExceptionWindow : Window
    {
        private const string TextDetailsOpen = "Show Details";
        private const string TextDetailsClose = "Hide Details";
        private const string TextCopy = "Copy";
        private const string TextCopied = "Copied..";

        public ExceptionWindow(string message, Exception ex)
        {
            Assert.NotNull(message, nameof(message));
            Assert.NotNull(ex, nameof(ex));
            this.InitializeComponent();

            this.Title = TextToWindowTitle.Convert("Error");
            this.BtnDetails.Content = TextDetailsOpen;
            this.BtnCopy.Content = TextCopy;

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
                this.BtnDetails.Content = TextDetailsOpen;
            }
            else
            {
                this.Height = 400;
                this.BtnCopy.Visibility = Visibility.Visible;
                this.TxtStacktrace.Visibility = Visibility.Visible;
                this.BtnDetails.Content = TextDetailsClose;
            }
        }

        private void BtnCopyOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Clipboard.SetText(this.TxtStacktrace.Text);

            if (string.Equals(this.BtnCopy.Content, TextCopy))
            {
                this.BtnCopy.Content = TextCopied;
                TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Delay(1000).ContinueWith(task => this.BtnCopy.Content = TextCopy, scheduler);
            }            
        }
    }
}
