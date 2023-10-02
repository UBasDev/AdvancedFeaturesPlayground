using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class IntegrationEvent
    {
        [JsonProperty] public Guid Id { get; private set; }
        [JsonProperty] public DateTime CreateDate { get; private set; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        [Newtonsoft.Json.JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreateDate = createDate;
        }
    }
}
