using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrainsForGreenFuture.Data
{
    public class TrainsDbContext : IdentityDbContext
    {
        public TrainsDbContext(DbContextOptions<TrainsDbContext> options)
            : base(options)
        {
        }
    }
}