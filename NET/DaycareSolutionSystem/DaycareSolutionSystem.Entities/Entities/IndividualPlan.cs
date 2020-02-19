using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("IndividualPlan")]
    public class IndividualPlan : EntityBase
    {
        public IndividualPlan()
        {
            AgreedClientActions = new HashSet<AgreedClientAction>();
        }

        public virtual Client Client { get; set; }

        public Guid ClientId { get; set; }

        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }

        public DateTime ValidFromDate { get; set; }

        public DateTime ValidUntilDate { get; set; }
    }
}
