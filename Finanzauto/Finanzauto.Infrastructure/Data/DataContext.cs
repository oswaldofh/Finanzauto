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

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePhoto> VehiclePhotos { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<VehicleAudit> VehicleAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Phase>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Brand>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Vehicle>().HasIndex(c => c.Plate).IsUnique();
            modelBuilder.Entity<VehiclePhoto>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<VehicleAudit>();

        }

    }
}
