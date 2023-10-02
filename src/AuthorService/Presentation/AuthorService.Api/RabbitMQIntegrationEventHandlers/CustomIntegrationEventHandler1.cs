using AuthorService.Api.RabbitMQIntegrationEvents;
using AuthorService.Application.Contexts;
using AuthorService.Domain.Entities;
using RabbitMQ;
using System.Text.Json;

namespace AuthorService.Api.RabbitMQIntegrationEventHandlers
{
    public class CustomIntegrationEventHandler1 : IIntegrationEventHandler<CustomIntegrationEvent1>
    {
        private readonly ApplicationDbContext _context;
        public CustomIntegrationEventHandler1(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handler(CustomIntegrationEvent1 @event)
        {
            var serializedNewAuthor1 = @event.SerializedNewAuthor;

            Author? deserializedNewAuthor1 = JsonSerializer.Deserialize<Author>(serializedNewAuthor1);

            await _context.Authors.AddAsync(deserializedNewAuthor1);
            await _context.SaveChangesAsync();
        }
    }
}
