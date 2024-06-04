using Finanzauto.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.Entities
{
    public class VehicleAudit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ActionAudit ActionAudit { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public User User { get; set; }

        [Required]
        [MaxLength(50)]
        public string PreviousValue { get; set; }

        [Required]
        [MaxLength(50)]
        public string NewValue { get; set; }
    }
}
