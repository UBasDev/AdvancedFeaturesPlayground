using AuthorService.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AuthorService.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RoleAuthorizeAttribute1 : AuthorizeAttribute
    {
        public RoleAuthorizeAttribute1(params AuthRole[] roles)
        {
            if (roles.Length > 0) Roles = String.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}
