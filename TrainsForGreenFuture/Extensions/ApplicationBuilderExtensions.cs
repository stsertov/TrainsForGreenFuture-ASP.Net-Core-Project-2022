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
            SeedEngineerUser(services);
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
            Task.Run (async () => 
            await ApplyRole(services, AdministratorRole))
                .GetAwaiter()
                .GetResult();          

            const string username = "abraham";
            const string adminPassword = "admin12";

            var user = new User
            {
                Email = username,
                FirstName = username,
                LastName = adminPassword,
                Company = "Trains For Green Future",
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

        public static void SeedEngineerUser(IServiceProvider services)
        {
            Task.Run(async () =>
            await ApplyRole(services, EngineerRole))
                .GetAwaiter()
                .GetResult();

            const string username = "engineer";
            const string engineerPassword = "engineer11";

            var user = new User
            {
                Email = username,
                FirstName = username,
                LastName = engineerPassword,
                Company = "Trains For Green Future",
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
                .AddToRoleAsync(user, EngineerRole))
                .GetAwaiter()
                .GetResult();
        }

        public static void SeedLocomotives(IServiceProvider services)
        {
            var locomotives = new []
            {
                new Locomotive(),
                new Locomotive(),
                new Locomotive()
            };


        }
        private static async Task ApplyRole(IServiceProvider services, string roleName)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync(roleName))
                return;

            var role = new IdentityRole { Name = roleName };

            await roleManager.CreateAsync(role);
        }
    }
}
