using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        [DataMember]
        [Key]
        public Guid Id { get; set; }
    }
}
