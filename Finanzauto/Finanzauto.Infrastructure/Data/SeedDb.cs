using Finanzauto.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finanzauto.Infrastructure.Data
{
    public class SeedDb
    {
        public SeedDb()
        {

        }
        public void AddCity(EntityTypeBuilder<City> entityTypeBuilder)
        {
            List<City> cities = new List<City>()
            {
                { new City { Id = 1, Name = "Medellín" } },
                { new City { Id = 2, Name = "Bogotá" } },
                { new City { Id = 3, Name = "Ciudad de México" } },
                { new City { Id = 4, Name = "Buenos Aires" } },
                { new City { Id = 5, Name = "Lima" } }
            };

            entityTypeBuilder.HasData(cities);
        }

    }
}
