using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositores;
using Ecom.infrastructure.Repositores.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
            services.AddScoped<IUnitOfWork,UnitOfWork>();
           services.AddSingleton<IImageMangmentService, UmageMangmentService>();
            IServiceCollection serviceCollection = services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            return services;
        }
    }
}
