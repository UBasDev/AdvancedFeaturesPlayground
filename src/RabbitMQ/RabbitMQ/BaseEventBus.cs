using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly IEventBusSubscriptionManager SubscriptionManager;
        public EventBusConfig eventBusConfig;

        protected BaseEventBus(IServiceProvider serviceProvider, EventBusConfig config)
        {
            eventBusConfig = config;
            ServiceProvider = serviceProvider;
            SubscriptionManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        public virtual string ProcessEventName(string eventName)
        {
            if (eventBusConfig.DeleteEventPrefix)
            {
                var index = eventName.IndexOf(eventBusConfig.EventNamePrefix, StringComparison.Ordinal);
                eventName = index > 0 ? eventName.Remove(index, eventBusConfig.EventNamePrefix.Length) : eventName;
            }

            if (eventBusConfig.DeleteEventSuffix)
            {
                var index = eventName.IndexOf(eventBusConfig.EventNameSuffix, StringComparison.Ordinal);
                eventName = index > 0 ? eventName.Remove(index, eventBusConfig.EventNameSuffix.Length) : eventName;
            }

            return eventName;
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }

        public virtual void Dispose()
        {
            eventBusConfig = null;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);
            var processed = false;
            if (SubscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = SubscriptionManager.GetHandlersForEvent(eventName);
                using (var scope = ServiceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions)
                    {
                        var handler = ServiceProvider.GetService(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType =
                            SubscriptionManager.GetEventTypeByName(
                                $"{eventBusConfig.EventNamePrefix}{eventName}{eventBusConfig.EventNameSuffix}");
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handler").Invoke(handler, new object[] { integrationEvent });
                    }
                }

                processed = true;
            }

            return processed;
        }

        public abstract void Publish(IntegrationEvent @event);
        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        public abstract void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}
