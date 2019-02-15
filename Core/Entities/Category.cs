using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
    }
}
