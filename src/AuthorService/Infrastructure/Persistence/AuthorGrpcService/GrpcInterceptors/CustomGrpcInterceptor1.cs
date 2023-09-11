using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AuthorGrpcService.GrpcInterceptors
{
    public class CustomGrpcInterceptor1:Interceptor
    {
        private readonly ILogger<CustomGrpcInterceptor1> _logger;
        public CustomGrpcInterceptor1(ILogger<CustomGrpcInterceptor1> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                _logger.LogInformation("Interceptor works!");
                return await continuation(request, context);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "{Type} {Status} {CreatedDate}", "Exception", "Error", DateTime.Now);
                throw;
            }
        }
    }
}
