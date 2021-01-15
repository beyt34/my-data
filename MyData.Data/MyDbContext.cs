using Microsoft.EntityFrameworkCore;

using MyData.Data.Domain;

namespace MyData.Data {
    public class MyDbContext : DbContext {
        private readonly string connectionString;

        public MyDbContext(string connectionString) {
            this.connectionString = connectionString;
        }

        public DbSet<City> City { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.EnableSensitiveDataLogging().UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<City>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
