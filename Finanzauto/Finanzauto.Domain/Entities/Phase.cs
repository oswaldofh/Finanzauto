using System.ComponentModel.DataAnnotations;

namespace Finanzauto.Domain.Entities
{
    public class Phase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
