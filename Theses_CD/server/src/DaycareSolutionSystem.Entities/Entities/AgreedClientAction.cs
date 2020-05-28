using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("AgreedClientAction")]
    public class AgreedClientAction : EntityBase
    {
        public AgreedClientAction()
        {
            RegisteredClientActions = new HashSet<RegisteredClientAction>();
            IsValid = true;
        }

        public Guid IndividualPlanId { get; set; }

        public virtual IndividualPlan IndividualPlan { get; set; }

        public Guid ActionId { get; set; }

        public virtual Action Action { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public DayOfWeek Day { get; set; }

        public int EstimatedDurationMinutes { get; set; }

        public TimeSpan PlannedStartTime { get; set; }

        public bool IsValid { get; set; }

        public virtual HashSet<RegisteredClientAction> RegisteredClientActions { get; set; }
    }
}
