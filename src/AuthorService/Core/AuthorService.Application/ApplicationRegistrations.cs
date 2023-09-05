using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorService.Application
{
    public static class ApplicationRegistrations
    {
        public static IServiceCollection AddJwtAuth1(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(options1 => //Burada applikasyonumuza authentication olarak `JWT`yi ekliyoruz
            {
                options1.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options1.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
                if (o.SecurityTokenValidators.FirstOrDefault() is JwtSecurityTokenHandler jwtSecurityTokenHandler)
                    jwtSecurityTokenHandler.MapInboundClaims = false;
                o.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
                o.TokenValidationParameters.NameClaimType = JwtClaimTypes.PreferredUserName;
                o.TokenValidationParameters.ValidateAudience = true;
                o.TokenValidationParameters.ValidIssuer = "https://localhost:7033";
                o.TokenValidationParameters.ValidAudience = "https://localhost:4200";
                o.TokenValidationParameters.ValidateIssuer = true;
                o.TokenValidationParameters.ValidateLifetime = true;
                o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                o.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nsaklnxuskaxnukassaxsaxasxasxasxasjkuasjkdajs"));
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
            });
            services.AddAuthorization();

            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, string serviceTitle)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = serviceTitle,
                    Description = serviceTitle,
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = string.Empty
                    }
                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Bearer Authentication",
                    Description = "Enter Bearer Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer"
                        }
                    },
                    System.Array.Empty<string>()
                }
            });
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.EnableAnnotations();
            });
            return services;
        }
    }
}
