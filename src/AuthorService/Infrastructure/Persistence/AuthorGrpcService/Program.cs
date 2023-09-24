using AuthorGrpcService.GrpcInterceptors;
using AuthorGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CustomGrpcInterceptor1>();
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = null;
    options.MaxSendMessageSize = null;
});

const string allowAllPolicy1 = "AllowAll1";
builder.Services.AddGrpcReflection(); //IOC içerisine GRPC reflection servisini ekler.

builder.Services.AddCors(o => o.AddPolicy(name: allowAllPolicy1, //Bu GRPC serverý için CORS policysi oluþtururuz.
    policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            .WithExposedHeaders("Grpc - Status", "Grpc - Message");
    }));

var app = builder.Build();
app.MapGrpcReflectionService();
app.UseCors(allowAllPolicy1);
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
// Configure the HTTP request pipeline.
app.MapGrpcService<AuthorService>().EnableGrpcWeb().RequireCors(allowAllPolicy1);
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
