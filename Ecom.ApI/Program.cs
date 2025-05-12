
using Ecom.ApI.Middleware;
using Ecom.infrastructure;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Ecom.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ecom.API", Version = "v1" });
            });
            //Register services
            builder.Services.AddInfrastructure(builder.Configuration);
            //register AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("allow-all", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular origin
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // ضروري عشان الكوكيز
                });
            });


            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddSwagerJwtAuth();

            var app = builder.Build();

            //add cors
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapSwagger();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecom.API v1");
                    c.RoutePrefix = string.Empty; // اجعل Swagger متاحًا في الصفحة الرئيسية
                });
            }
            //app.UseMiddleware<ExceptionsMiddleware>();


            //for error handling =>if we not found page rediriect to error page
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseCors("allow-all");
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseMiddleware<ReadJwtTokenFromCoockies>();
            app.UseAuthentication();
            app.UseAuthorization();
            //add roels

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "User", "Manager" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            app.MapControllers();



            app.Run();
        }
    }
}
