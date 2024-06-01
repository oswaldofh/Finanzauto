using Finanzauto.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finanzauto.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
            ModelConfig(modelBuilder);
        }

        private void ModelConfig(ModelBuilder builder)
        {
            var seedDb = new SeedDb();
            seedDb.AddCity(builder.Entity<City>());
        }
    
    }
}
