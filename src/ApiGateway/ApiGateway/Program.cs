using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
var cancellationToken1 = new CancellationToken();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).AddEnvironmentVariables().Build();

builder.Services.AddOcelot(configuration).AddPolly();
var app = builder.Build();

// Configure the HTTP request pipeline.

await app.UseOcelot().WaitAsync(cancellationToken1);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
