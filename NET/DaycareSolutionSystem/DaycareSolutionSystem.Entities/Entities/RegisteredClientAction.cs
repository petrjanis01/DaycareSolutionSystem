using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("RegisteredClientAction")]
    public class RegisteredClientAction : EntityBase
    {
        public Guid ClientId { get; set; }

        public virtual Client Client { get; set; }

        public Guid ActionId { get; set; }

        public virtual Action Action { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        // nullable due to adhoc actions
        public Guid? AgreedClientActionId { get; set; }

        public int? EstimatedDurationMinutes { get; set; }

        public virtual AgreedClientAction AgreedClientAction { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCanceled { get; set; }

        public DateTime? ActionStartedDateTime { get; set; }

        public DateTime? ActionFinishedDateTime { get; set; }

        public DateTime PlannedStartDateTime { get; set; }

        public string Comment { get; set; }

        public virtual Picture Photo { get; set; }

        public Guid? PhotoId { get; set; }
    }
}
