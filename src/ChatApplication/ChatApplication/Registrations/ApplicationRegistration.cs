using ChatApplication.Hubs;

namespace ChatApplication.Registrations
{
    public static class ApplicationRegistration
    {
        private const string PolicyName = "AllowAll";

        public static IServiceCollection AddMyCustomRegistrations(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin =>
                    {
                        return true;
                    })
                    .AllowCredentials(); //Socket işlemlerinde Credentiallara izin verilmesi gerekir.
                });
            });
            return services;
        }
        public static void AddMyCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseCors(PolicyName);
        }
    }
}
