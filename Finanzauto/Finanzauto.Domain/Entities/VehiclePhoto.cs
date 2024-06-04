using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.Entities
{
    public class VehiclePhoto
    {
        [Key]
        public int Id { get; set; }
        public string? Image { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
