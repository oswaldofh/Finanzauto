﻿using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.DTOs
{
    public class CityDto : CreateCityDto
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }
    }
}
