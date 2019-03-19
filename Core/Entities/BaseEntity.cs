using SolrNet.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        [SolrUniqueKey]
        public Guid Id { get; set; }

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

        [ConcurrencyCheck]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
