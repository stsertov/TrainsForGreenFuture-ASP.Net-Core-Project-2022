namespace TrainForGreenFuture.Test
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using TrainsForGreenFuture.Infrastructure.Data;

    public class InMemoryDbContext
    {
        private SqliteConnection connection;
        private DbContextOptions<TrainsDbContext> dbContextOptions;


        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<TrainsDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new TrainsDbContext(dbContextOptions);

            context.Database.EnsureCreated();
        }

        public TrainsDbContext CreateDb() 
            => new TrainsDbContext(dbContextOptions);

        public void Dispose()
            => connection.Dispose();
    }
}
