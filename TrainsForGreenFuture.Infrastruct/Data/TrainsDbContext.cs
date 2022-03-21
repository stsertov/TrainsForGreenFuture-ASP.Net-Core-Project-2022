namespace TrainsForGreenFuture.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class TrainsDbContext : IdentityDbContext
    {
        public TrainsDbContext(DbContextOptions<TrainsDbContext> options)
            : base(options)
        {
        }
    }
}