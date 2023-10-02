using AuthorService.Application.Interfaces.Hangfire;
using AuthorService.Application.Interfaces.Redis;
using AuthorService.Persistence.Hangfire;
using AuthorService.Persistence.Redis;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Builder;
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
        public static IServiceCollection AddHangfire1(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISyncAuthors, SyncAuthors>();
            return services;
        }
        public static void StartHangFireJobs(this IApplicationBuilder app)
        {
            var jobManager = new RecurringJobManager();
            jobManager.AddOrUpdate(
                "sync-authors",
                Job.FromExpression(() => new SyncAuthors().Test1()),
                "*/50 * * * *"
                );
        }
    }
}
