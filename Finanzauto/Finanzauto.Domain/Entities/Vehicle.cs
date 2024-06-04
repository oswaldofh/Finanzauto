using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Plate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Color { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string Line { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string Mileage { get; set; }

        [Required]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Observation { get; set; }

        [ForeignKey("PhaseId")]
        public int PhaseId { get; set; }

        public Phase Phase { get; set; }
        public ICollection<VehicleAudit> VehicleAudits { get; set; }
        public ICollection<VehiclePhoto> VehiclePhotos { get; set; }
    }
}
