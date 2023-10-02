using AuthorService.Api.Controllers;
using AuthorService.Api.RabbitMQIntegrationEventHandlers;
using AuthorService.Api.RabbitMQIntegrationEvents;
using AuthorService.Application;
using AuthorService.Application.Contexts;
using AuthorService.Application.Interfaces.Hangfire;
using AuthorService.Persistence;
using AuthorService.Persistence.Hangfire;
using Hangfire;
using Hangfire.Common;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.Configuration;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient<AuthorController>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddJwtAuth1(builder.Configuration);
builder.Services.AddRedis1(builder.Configuration);
builder.Services.AddHangfire1(builder.Configuration);
// RABBITMQ
builder.Services.AddTransient<CustomIntegrationEventHandler1>();
builder.Services.AddSingleton<IEventBus>(sp => new EventBusRabbitMq(sp, new EventBusConfig
{
    DefaultTopicName = "MyProject1",
    ConnectionRetryCount = 5,
    EventNameSuffix = "IntegrationEvent1",
    SubscriberClientAppName = "AuthorService1",
    Connection = new
    {
        HostName = "localhost",
        Port = 5672,
        /*
        UserName= "username1",
        Password= "password1"
        */
    }
}));
ServiceProvider sp1 = builder.Services.BuildServiceProvider();
IEventBus? eventBus1 = sp1.GetRequiredService<IEventBus>();
eventBus1.Subscribe<CustomIntegrationEvent1, CustomIntegrationEventHandler1>();
// RABBITMQ

builder.Services.AddHangfire(options =>
{
    options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
    options.UseSimpleAssemblyNameTypeSerializer();
    options.UseRecommendedSerializerSettings();
    options.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection("User ID = postgres; Password = admin; Server = localhost; Port = 5432; Database = NET2;", connection =>
        {

        });
    });
});
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 1;
    options.HeartbeatInterval = TimeSpan.FromSeconds(10);

});
builder.Services.AddSwagger("Test Api Title1");
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    AppPath = null,
    DashboardTitle = "Identity Job Service Hangfire Dashboard",
    Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                User = "username1",
                Pass = "password1"
            }
        }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.StartHangFireJobs();
app.Run();
