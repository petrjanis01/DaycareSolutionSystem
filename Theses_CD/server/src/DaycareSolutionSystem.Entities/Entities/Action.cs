using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Action")]
    public class Action : EntityBase
    {
        public Action()
        {
            AgreedClientActions = new HashSet<AgreedClientAction>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GeneralDescription { get; set; }

        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }
    }
}
