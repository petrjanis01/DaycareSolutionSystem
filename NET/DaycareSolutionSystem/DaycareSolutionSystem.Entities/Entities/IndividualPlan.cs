using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("IndividualPlan")]
    public class IndividualPlan : EntityBase
    {
        public IndividualPlan()
        {
            AgreedClientActions = new HashSet<AgreedClientAction>();
        }

        [DataMember]
        public virtual Client Client { get; set; }

        [DataMember]
        public Guid ClientId { get; set; }

        [DataMember]
        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }

        [DataMember]
        public DateTime ValidFromDate { get; set; }

        [DataMember]
        public DateTime ValidUntilDate { get; set; }
    }
}
