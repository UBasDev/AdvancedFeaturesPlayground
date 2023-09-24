using ChatApplicationServer.Hubs;
using ChatApplicationServer.Models.AppSettings;
using ChatApplicationServer.Registrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Settings"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddControllers();
var appSettings = new AppSettings();
configuration.Bind(nameof(AppSettings), appSettings);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
if(appSettings.IsSwaggerActive) builder.Services.AddSwaggerGen();
builder.Services.AddCommonRegistrations(appSettings);
builder.Services.AddSignalRRegistrations();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (appSettings.IsSwaggerActive)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.AddCommonMiddlewares();
app.MapHub<ChatHub>("/ServerChatHub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
