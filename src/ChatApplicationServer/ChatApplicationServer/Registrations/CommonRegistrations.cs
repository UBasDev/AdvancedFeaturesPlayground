using ChatApplicationServer.Models.AppSettings;

namespace ChatApplicationServer.Registrations
{
    public static class CommonRegistrations
    {
        static readonly string CorsPolicyName = "AllowAll";
        public static IServiceCollection AddCommonRegistrations(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton<AppSettings>();
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin =>
                    {
                        return true;
                    })
                    .AllowCredentials();
                });
            });
            return services;
        }
        public static void AddCommonMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseCors(CorsPolicyName);
        }
    }
}
