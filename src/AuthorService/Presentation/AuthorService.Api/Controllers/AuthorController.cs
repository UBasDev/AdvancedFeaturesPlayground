using AuthorService.Application.Attributes;
using AuthorService.Application.Domain.Entity;
using AuthorService.Application.Enums;
using AuthorService.Application.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace AuthorService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private RequestedUser RequestedUser
        {
            get
            {
                var idClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.Id) select new { c.Value };
                var roleClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.Role) select new { c.Value };
                var preferredUsernameClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.PreferredUserName) select new { c.Value };
                var fullnameClaims = from c in User.Claims.Where(f => f.Type == "fullname") select new { c.Value };
                return new RequestedUser
                {
                    Id = idClaims.FirstOrDefault()!.Value,
                    PreferredUsername = preferredUsernameClaims.FirstOrDefault()!.Value,
                    Role = (AuthRole)Enum.Parse(typeof(AuthRole), roleClaims.FirstOrDefault()!.Value, true),
                    FullName = fullnameClaims.FirstOrDefault()!.Value,
                };
            }
        }
        public static List<Author> authors = new List<Author>()
        {
            new()
            {
                Id = 1,
                AuthorName = "author1",
                Age= 11
            },
            new()
            {
                Id = 2,
                AuthorName = "author2",
                Age= 22
            },
            new()
            {
                Id = 3,
                AuthorName = "author3",
                Age= 33
            }
        };
        [HttpGet("[action]")]
        public IActionResult GetToken()
        {
            return Ok(TokenGenerator());
        }
        private string TokenGenerator() //Bu metod sayesinde token generate ederiz.
        {
            var tokenExpireDate = new TimeSpan(180, 0, 0, 0, 0);
            var claims = new List<Claim>
        {
            new(JwtClaimTypes.Id, "213129"),
            new(JwtClaimTypes.PreferredUserName, "Preferred1 Ahmet1"),
            new("fullname", "Ahmet1"),
            new(JwtClaimTypes.Role, "CEO"),
        };
            var token = GetJwtToken1(tokenExpireDate, claims);
            return token;
        }
        private string GetJwtToken1(TimeSpan expiration, IEnumerable<Claim> claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nsaklnxuskaxnukassaxsaxasxasxasxasjkuasjkdajs")); //Tokenının security keyini belirttik.

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7033",

           audience: "https://localhost:4200",
        expires: DateTime.UtcNow.Add(expiration),

                claims: claims,

                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("[action]")]
        [RoleAuthorizeAttribute1(AuthRole.CEO, AuthRole.OC)]
        public IActionResult Test1()
        {
            var x = RequestedUser;
            return Ok();
        }
        [HttpGet("[action]")]
        [FullnameAuthorizeAttribute1("Ahmet1", "Mehmet1")]
        public IActionResult Test2()
        {
            return Ok();
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult Test3()
        {
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test4()
        {
            var x1 = _httpContextAccessor.HttpContext?.Request;
            return Ok();
        }
    }
}
