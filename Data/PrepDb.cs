using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService <AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any() )
            {
                Console.WriteLine ("--> Seeding Data");

                context.Platforms.AddRange  (
                    new Models.Platform (){Name="DotNet", Publisher ="Microsoft", Cost="100"},
                    new Models.Platform (){Name="Redis", Publisher ="RedisCorp", Cost="Free"}
                );
                context.SaveChanges ();

            }
            else
            {
                Console.WriteLine ("--> there are some platforms in DB already");
            }
            //context.
        }
    }

}