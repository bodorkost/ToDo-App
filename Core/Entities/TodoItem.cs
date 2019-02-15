using System;
using System.ComponentModel.DataAnnotations;
using Core.Types;

namespace Core.Entities
{
    public class TodoItem
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [StringLength(50)]
        public string Responsible { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        public Status Status { get; set; }

        public Category Category { get; set; }

        public Guid? ParentId { get; set; }

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
