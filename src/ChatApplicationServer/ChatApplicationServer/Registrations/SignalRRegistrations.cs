namespace ChatApplicationServer.Registrations
{
    public static class SignalRRegistrations
    {
        public static IServiceCollection AddSignalRRegistrations(this IServiceCollection services)
        {
            services.AddSignalR(configuration =>
            {
                configuration.EnableDetailedErrors = true;
                configuration.MaximumReceiveMessageSize = null;
                configuration.HandshakeTimeout = TimeSpan.FromSeconds(15);
            });
            return services;
        }
    }
}
