using System;
using System.Collections.Generic;

namespace PboManager.Services.EventBus
{
    public class EventBusImpl : IEventBus
    {
        private readonly ICollection<IDisposable> subscrptions = new List<IDisposable>();

        public void Publish<T>(T action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (IDisposable disposable in this.subscrptions)
            {
                if (disposable is EventBusSubscription<T> subscription)
                {
                    subscription.Invoke(action);
                }
            }
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var subscription = new EventBusSubscription<T>(this, callback);
            this.subscrptions.Add(subscription);
            return subscription;
        }

        private class EventBusSubscription<T> : IDisposable
        {
            private readonly Action<T> handleInvoke;
            private readonly EventBusImpl eventBus;

            public EventBusSubscription(EventBusImpl eventBus, Action<T> handleInvoke)
            {
                this.eventBus = eventBus;
                this.handleInvoke = handleInvoke;
            }

            internal void Invoke(T action)
            {
                this.handleInvoke.Invoke(action);
            }

            public void Dispose()
            {
                this.eventBus.subscrptions.Remove(this);
            }
        }
    }
}