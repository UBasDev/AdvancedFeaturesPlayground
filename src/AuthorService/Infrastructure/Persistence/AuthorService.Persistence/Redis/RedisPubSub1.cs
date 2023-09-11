using AuthorService.Application.Interfaces.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Persistence.Redis
{
    public class RedisPubSub1 : IRedisPubSub1
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisPubSub1(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public void Publish(string channelName, string message)
        {
            var pubSubConnection = _connectionMultiplexer.GetSubscriber();
            pubSubConnection.Publish(channelName, message);
        }
        public ChannelMessageQueue Subscribe(string channelName)
        {
            var pubSubConnection = _connectionMultiplexer.GetSubscriber();
            return pubSubConnection.Subscribe(channelName);
        }
    }
}
