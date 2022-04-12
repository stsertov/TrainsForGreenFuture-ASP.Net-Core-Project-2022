namespace TrainsForGreenFuture.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public class TrainsDbContext : IdentityDbContext<User>
    {
        public TrainsDbContext(DbContextOptions<TrainsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Locomotive> Locomotives { get; set; }

        public DbSet<TrainCar> TrainCars { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Interrail> Interrails { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Renovation> Renovations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Locomotive>(l =>
            {
                l.HasOne(i => i.Interrail)
                .WithMany()
                .HasForeignKey(k => k.InterrailId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<TrainCar>(tc =>
            {
                tc.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(k => k.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

                tc.HasOne(i => i.Interrail)
                .WithMany()
                .HasForeignKey(k => k.InterrailId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Train>(t =>
            {
                t.HasOne(i => i.Interrail)
                .WithMany()
                .HasForeignKey(k => k.InterrailId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>(o =>
            {
                o.HasOne(u => u.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                o.HasOne(l => l.Locomotive)
                .WithMany()
                .HasForeignKey(k => k.LocomotiveId)
                .OnDelete(DeleteBehavior.Restrict);

                o.HasOne(tc => tc.TrainCar)
                .WithMany()
                .HasForeignKey(k => k.TrainCarId)
                .OnDelete(DeleteBehavior.Restrict);

                o.HasOne(t => t.Train)
                .WithMany()
                .HasForeignKey(k => k.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Renovation>(r =>
            {
                r.HasOne(r => r.User)
                .WithMany(u => u.Renovations)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                r.HasOne(l => l.Locomotive)
                .WithMany()
                .HasForeignKey(k => k.LocomotiveId)
                .OnDelete(DeleteBehavior.Cascade);

                r.HasOne(l => l.TrainCar)
                .WithMany()
                .HasForeignKey(k => k.TrainCarId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Locomotive>()
                .Property(b => b.Price)
                .HasPrecision(12, 2);

            builder.Entity<TrainCar>()
                .Property(b => b.Price)
                .HasPrecision(12, 2);

            builder.Entity<Train>()
                .Property(b => b.Price)
                .HasPrecision(12, 2); 
            
            builder.Entity<Order>()
                .Property(b => b.AdditionalInterrailTax)
                .HasPrecision(12, 2);

            builder.Entity<Order>()
                .Property(b => b.AdditionalLuxuryLevelTax)
                .HasPrecision(12, 2);

            builder.Entity<Renovation>()
                .Property(b => b.Price)
                .HasPrecision(12, 2);


            base.OnModelCreating(builder);
        }
    }
}