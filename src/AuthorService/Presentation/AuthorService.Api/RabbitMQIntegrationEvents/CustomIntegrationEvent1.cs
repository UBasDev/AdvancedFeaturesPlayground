using AuthorService.Domain.Entities;
using RabbitMQ;

namespace AuthorService.Api.RabbitMQIntegrationEvents
{
    public class CustomIntegrationEvent1 : IntegrationEvent
    {
        public string SerializedNewAuthor { get; set; }

        public CustomIntegrationEvent1(string newSerializedAuthor)
        {
            SerializedNewAuthor = newSerializedAuthor;
        }
    }
}
