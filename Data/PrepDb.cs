using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        private static IWebHostEnvironment _env;

        public static void PrepPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _env = env;
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Apply migration ");
                try
                {
                    context.Database.Migrate(); 
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"--> Apply migration ERROR {ex.Message}");
                }
                
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data");

                context.Platforms.AddRange(
                    new Models.Platform() { Name = "DotNet", Publisher = "Microsoft", Cost = "100" },
                    new Models.Platform() { Name = "Redis", Publisher = "RedisCorp", Cost = "Free" },
                    new Models.Platform() { Name = "K8s", Publisher = "Google", Cost = "Free" }
                );
                context.SaveChanges();

            }
            else
            {
                Console.WriteLine("--> there are some platforms in DB already");
            }

        }
    }

}