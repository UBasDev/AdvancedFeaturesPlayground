using Grpc.Core;
using Microservice1.protos;

namespace AuthorGrpcService.Services
{
    public class AuthorService : AuthorServiceApi.AuthorServiceApiBase
    {
        public override async Task<GetAllAuthorsByMinAgeResponse> GetAllAuthorsByMinAgeGrpcService(GetAllAuthorsByMinAgeRequest request, ServerCallContext context)
        {
            if (request.AuthorAge < 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            }
            var response1 = new GetAllAuthorsByMinAgeResponse();
            var responseBody1 = new List<GetAllAuthorsByMinAge>();
            var object1 = new GetAllAuthorsByMinAge()
            {
                AuthorAge = request.AuthorAge,
                AuthorId = 1,
                AuthorName = "Author1"
            };
            var object2 = new GetAllAuthorsByMinAge()
            {
                AuthorAge = request.AuthorAge,
                AuthorId = 2,
                AuthorName = "Author2"
            };
            var object3 = new GetAllAuthorsByMinAge()
            {
                AuthorAge = request.AuthorAge,
                AuthorId = 3,
                AuthorName = "Author3"
            };
            responseBody1.Add(object1);
            responseBody1.Add(object2);
            responseBody1.Add(object3);
            response1.GetAllAuthorsByMinAge.Add(responseBody1);
            return await Task.FromResult(response1);
        }
    }
}
