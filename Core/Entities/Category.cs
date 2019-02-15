using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Category
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        public Guid CreatedById { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        public Guid ModifiedById { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deleted { get; set; }

        public Guid DeletedById { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
