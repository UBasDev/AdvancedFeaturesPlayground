using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace AuthorService.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CustomAttributeAsync1 : Attribute, IAsyncActionFilter
    {
        private int _property1;
        private string _property2;
        private string[] _property3;
        public CustomAttributeAsync1(int property1, string property2, string[] property3)
        {
            _property1 = property1;
            _property2 = property2;
            _property3 = property3;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool bool1 = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader1);
            string token1 = authorizationHeader1.ToString().Split(' ')[1];
            var decodedToken1 = new JwtSecurityTokenHandler().ReadToken(token1) as JwtSecurityToken;
            var roleClaims1 = decodedToken1?.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value;
            var fullnameClaims1 = decodedToken1?.Claims.FirstOrDefault(c => c.Type == "fullname")?.Value;
        }
    }
}
