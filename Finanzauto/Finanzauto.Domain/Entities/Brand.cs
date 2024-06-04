using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.Entities
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        //public ICollection<Vehicle> Vehicles { get; set; }
    }
}
