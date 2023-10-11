using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace BookService.Api
{
    public class CustomInterceptor1 : Attribute, IAsyncResultFilter
    {
        private List<string> RequiredRoles { get; set; } = new List<string>();
        public CustomInterceptor1(params string[] requiredRoles)
        {
            foreach (var currentRole in requiredRoles)
            {
                RequiredRoles.Add(currentRole);
            }
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine("CustomInterceptor1: {0}", string.Join(",", RequiredRoles));
            bool isUserExists = context.HttpContext.Request.Headers.TryGetValue("header1", out StringValues header1Value);
            if (isUserExists is not true) throw new ArgumentException("Giremen guzum");
        }
    }
    public class CustomInterceptor2 : Attribute, IAsyncAuthorizationFilter
    {
        private List<string> RequiredRoles { get; set; } = new List<string>();
        public CustomInterceptor2(params string[] requiredRoles)
        {
            foreach (var currentRole in requiredRoles)
            {
                RequiredRoles.Add(currentRole);
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine("CustomInterceptor2: {0}", string.Join(",", RequiredRoles));
            bool isUserExists = context.HttpContext.Request.Headers.TryGetValue("header1", out StringValues header1Value);
            if (!isUserExists) throw new ArgumentException("Giremen guzum");

        }
    }
    public class CustomInterceptor3 : Attribute, IAsyncActionFilter
    {
        private List<string> RequiredRoles { get; set; } = new List<string>();
        public CustomInterceptor3(params string[] requiredRoles)
        {
            foreach (var currentRole in requiredRoles)
            {
                RequiredRoles.Add(currentRole);
            }
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("CustomInterceptor3: {0}", string.Join(",", RequiredRoles));
            bool isUserExists = context.HttpContext.Request.Headers.TryGetValue("header1", out StringValues header1Value);
            if (isUserExists is not true) throw new ArgumentException("Giremen guzum");
        }
    }
    public class CustomInterceptor4 : Attribute, IAsyncResourceFilter
    {
        private List<string> RequiredRoles { get; set; } = new List<string>();
        public CustomInterceptor4(params string[] requiredRoles)
        {
            foreach (var currentRole in requiredRoles)
            {
                RequiredRoles.Add(currentRole);
            }
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("CustomInterceptor4: {0}", string.Join(",", RequiredRoles));
            bool isUserExists = context.HttpContext.Request.Headers.TryGetValue("header1", out StringValues header1Value);
            if (isUserExists is not true) throw new ArgumentException("Giremen guzum");
        }
    }
}
