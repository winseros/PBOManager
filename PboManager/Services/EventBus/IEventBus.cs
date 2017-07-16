using System;

namespace PboManager.Services.EventBus
{
    public interface IEventBus
    {
        void Publish<T>(T action);

        IDisposable Subscribe<T>(Action<T> callback);
    }
}