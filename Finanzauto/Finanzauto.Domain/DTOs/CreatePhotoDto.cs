using Finanzauto.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.DTOs
{
    public class CreatePhotoDto
    {
        public string? Image { get; set; }
        public int VehicleId { get; set; }
    }
}
