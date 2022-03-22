namespace TrainsForGreenFuture.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public class TrainsDbContext : IdentityDbContext
    {
        public TrainsDbContext(DbContextOptions<TrainsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Locomotive> Locomotives { get; set; }

        public DbSet<TrainCar> TrainCars { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}