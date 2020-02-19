using System;
using System.ComponentModel.DataAnnotations;

namespace DaycareSolutionSystem.Database.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
    }
}
