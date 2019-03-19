using System;
using System.ComponentModel.DataAnnotations;
using Core.Types;
using SolrNet.Attributes;

namespace Core.Entities
{
    public class TodoItem : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [SolrField]
        public string Name { get; set; }

        [StringLength(500)]
        [SolrField]
        public string Description { get; set; }

        [Required]
        [SolrField]
        public Priority Priority { get; set; }

        [StringLength(50)]
        [SolrField]
        public string Responsible { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [SolrField]
        public Status Status { get; set; }

        public Guid? CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid? ParentId { get; set; }

        public int WorkHours
        {
            get
            {
                int workHours = 0;

                for (var i = DateTime.Now; i < Deadline; i = i.AddHours(1))
                {
                    if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                        {
                            ++workHours;
                        }
                    }
                }

                return workHours;
            }
        }
    }
}
