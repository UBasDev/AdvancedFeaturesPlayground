using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
    {
        Task Handler(TIntegrationEvent @event);
    }

    public interface IntegrationEventHandler
    {
    }
}
