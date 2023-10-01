using AuthorService.Api.Controllers;
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
    options.WorkerCount = 8;
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
