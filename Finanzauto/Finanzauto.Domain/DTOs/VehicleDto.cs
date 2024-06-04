using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.DTOs
{
    public class VehicleDto : CreateVehicleDto
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }

    }
}
