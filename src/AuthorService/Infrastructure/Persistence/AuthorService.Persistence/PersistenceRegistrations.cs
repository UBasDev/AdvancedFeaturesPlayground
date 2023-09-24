using AuthorService.Application.Interfaces.Redis;
using AuthorService.Persistence.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuthorService.Persistence
{
    public static class PersistenceRegistrations
    {
        public static IServiceCollection AddRedis1(this IServiceCollection services, IConfiguration configuration)
        {
            string redisConnectionString1 = configuration.GetConnectionString("RedisConnectionString") ?? String.Empty;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString1;
            });
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString1)
                );
            services.AddScoped<IRedisCacheRepository1, RedisCacheRepository1>();
            services.AddScoped<IRedisPubSub1, RedisPubSub1>();
            return services;
        }
    }
}
