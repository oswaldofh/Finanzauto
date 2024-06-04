using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.Entities
{
    public class Client
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Document { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        [MaxLength(15)]
        public string CellPhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
