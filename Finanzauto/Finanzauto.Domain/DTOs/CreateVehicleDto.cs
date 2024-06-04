using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.DTOs
{
    public class CreateVehicleDto
    {
        [Display(Name = "Placa")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plate { get; set; }

        [Display(Name = "Color")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BrandId { get; set; }

        [Display(Name = "Fase")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PhaseId { get; set; }

        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Line { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Year { get; set; }

        [Display(Name = "Kilometraje")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Mileage { get; set; }
        

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }
        
        [Display(Name = "Observación")]
        public string? Observation { get; set; }
        
        [Display(Name = "Imagenes")]
        public List<IFormFile?> Images { get; set; }
    }
}
