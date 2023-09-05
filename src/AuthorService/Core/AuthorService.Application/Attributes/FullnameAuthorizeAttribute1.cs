using AuthorService.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class FullnameAuthorizeAttribute1 : AuthorizeAttribute, IAuthorizationFilter
    {
        public ISet<string> AllowedFullnames { get; set; } = new HashSet<string>();
        public FullnameAuthorizeAttribute1(params string[] fullnames)
        {
            //if (fullnames.Length > 0) Roles = String.Join(",", fullnames.Select(r => r));
            if (fullnames.Length > 0)
            {
                foreach (string currentFullname in fullnames)
                {
                    AllowedFullnames.Add(currentFullname);
                }
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = false;
            var fullnameClaims = from c in context.HttpContext.User.Claims.Where(f => f.Type == "fullname") select new { c.Value };
            if (fullnameClaims == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            foreach (var currentFullnameClaim in fullnameClaims)
            {
                if (AllowedFullnames.Contains(currentFullnameClaim.Value.ToString()))
                {
                    result = true;
                    break;
                }
            }
            if(!result) context.Result = new UnauthorizedResult();
        }
    }
}
