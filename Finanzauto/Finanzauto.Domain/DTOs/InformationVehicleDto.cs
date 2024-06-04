using Finanzauto.Domain.Entities;
using System.Text.Json.Serialization;

namespace Finanzauto.Domain.DTOs
{
    public class InformationVehicleDto
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public string NameBrand => Brand.Name;
        public string Line { get; set; }
        public int Year { get; set; }
        public string Mileage { get; set; }
        public decimal Price { get; set; }
        public string? Observation { get; set; }
        [JsonIgnore]
        public Phase Phase { get; set; }
        public int PhaseId { get; set; }
        public string NamePhase => Phase.Name;
        public ICollection<PhotoDto> Images { get; set; }

    }
}
