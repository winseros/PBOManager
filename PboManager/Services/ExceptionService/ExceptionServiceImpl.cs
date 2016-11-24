using System;

namespace PboManager.Services.ExceptionService
{
    public class ExceptionServiceImpl : IExceptionService
    {
        public void ReportException(string message, Exception ex)
        {
            new ExceptionWindow(message, ex).Show();
        }
    }
}