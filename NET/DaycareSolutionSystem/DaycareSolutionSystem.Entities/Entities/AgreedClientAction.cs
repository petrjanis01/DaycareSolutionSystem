using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("AgreedClientAction")]
    public class AgreedClientAction : EntityBase
    {
        public AgreedClientAction()
        {
            RegisteredClientActions = new HashSet<RegisteredClientAction>();
        }

        [DataMember]
        public Guid IndividualPlanId { get; set; }

        [DataMember]
        public virtual IndividualPlan IndividualPlan { get; set; }

        [DataMember]
        public Guid ActionId { get; set; }

        [DataMember]
        public virtual Action Action { get; set; }

        [DataMember]
        public Guid EmployeeId { get; set; }

        [DataMember]
        public virtual Employee Employee { get; set; }

        [DataMember]
        public string ClientActionSpecificDescription { get; set; }

        [DataMember]
        public DayOfWeek Day { get; set; }

        [DataMember]
        public int EstimatedDuration { get; set; }

        [DataMember]
        public virtual HashSet<RegisteredClientAction> RegisteredClientActions { get; set; }
    }
}
