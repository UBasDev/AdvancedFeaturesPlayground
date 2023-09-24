
using StackExchange.Redis;

namespace AuthorService.Application.Interfaces.Redis
{
    public interface IRedisPubSub1
    {
        public void Publish(string channelName, string message);
        public ChannelMessageQueue Subscribe(string channelName);
    }
}
