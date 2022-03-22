namespace TrainsForGreenFuture.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder DatabaseCreator(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);


            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<TrainsDbContext>();

            data.Database.Migrate();
        }

        public static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<TrainsDbContext>();

            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Passenger" },
                new Category { Name = "Cargo" },
                new Category { Name = "Sleeper"},
                new Category { Name = "Restaurant"}
            });

            data.SaveChanges();
        }
    }
}
