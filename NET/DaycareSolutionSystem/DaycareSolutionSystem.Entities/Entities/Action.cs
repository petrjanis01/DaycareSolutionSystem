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

        [DataMember]
        [Required]
        [MaxLength(StringMaxLengthConstants.StringLengthLong)]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [MaxLength(StringMaxLengthConstants.StringLengthContent)]
        public string GeneralDescription { get; set; }

        [DataMember]
        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }
    }
}
