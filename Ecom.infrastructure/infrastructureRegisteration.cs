using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositores;
using Ecom.infrastructure.Repositores.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageMangmentService, UmageMangmentService>();
            IServiceCollection serviceCollection = services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            return services;
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["Jwt:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
           };
       });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });
        }


        public static void AddSwagerJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {

                o.AddSecurityDefinition("Barer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Barer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Description = "Enter the Jwt Key "
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference=new OpenApiReference()
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Barer",
                        },
                        Name="Barer",
                        In=ParameterLocation.Header
                    },
                    new List<string>()
                    }
                });
            });
        }
    }

}
