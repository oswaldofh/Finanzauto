using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.DTOs
{
    public class VehiclePhaseDto
    {
        [Display(Name = "Fase"),]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int PhaseId { get; set; }
    }
}
