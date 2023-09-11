
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Interfaces.Redis
{
    public interface IRedisPubSub1
    {
        public void Publish(string channelName, string message);
        public ChannelMessageQueue Subscribe(string channelName);
    }
}
