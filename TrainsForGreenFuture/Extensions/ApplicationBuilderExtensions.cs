namespace TrainsForGreenFuture.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    using static Areas.RolesConstants;
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder DatabaseCreator(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedInterrails(services);
            SeedAdminUser(services);
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

        public static void SeedInterrails(IServiceProvider services)
        {
            var data = services.GetRequiredService<TrainsDbContext>();

            data.Interrails.AddRange(new[]
            {
                new Interrail { Length = 760 },
                new Interrail { Length = 1435 },
                new Interrail { Length = 1520 }
            });

            data.SaveChanges();
        }

        public static void SeedAdminUser(IServiceProvider services)
        {
            const string username = "abraham";
            const string adminPassword = "admin12";

            var user = new User
            {
                Email = username,
                FirstName = username,
                LastName = adminPassword,
                Company = username + adminPassword,
                UserName = username
            };

            var db = services
                .GetRequiredService<TrainsDbContext>();

            if (db.Users.Any(x => x.UserName == username))
                return;

            db.Users.Add(user);
            db.SaveChanges();

            var userManager = services
                .GetRequiredService<UserManager<User>>();

            Task.Run(async () => 
                await userManager
                .AddToRoleAsync(user, AdministratorRole))
                .GetAwaiter()
                .GetResult();
        }
    }
}
