using System;

namespace PboManager.Services.ExceptionService
{
    public interface IExceptionService
    {
        void ReportException(string message, Exception ex);
    }
}
