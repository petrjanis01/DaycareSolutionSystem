using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("RegisteredClientAction")]
    public class RegisteredClientAction : EntityBase
    {
        [DataMember]
        public Guid ClientId { get; set; }

        [DataMember]
        public virtual Client Client { get; set; }

        [DataMember]
        public Guid EmployeeId { get; set; }

        [DataMember]
        public virtual Employee Employee { get; set; }

        [DataMember]
        public Guid AgreedClientActionId { get; set; }

        [DataMember]
        public virtual AgreedClientAction AgreedClientAction { get; set; }

        [DataMember]
        public bool IsCompleted { get; set; }

        [DataMember]
        public bool IsCanceled { get; set; }

        [DataMember]
        public DateTime? ActionStartedDateTime { get; set; }

        [DataMember]
        public DateTime? ActionFinishedDateTime { get; set; }

        [DataMember]
        public DateTime PlannedStartDateTime { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthContent)]
        public string Comment { get; set; }

        [DataMember]
        public virtual Picture Photo { get; set; }

        [DataMember]
        public Guid? PhotoId { get; set; }
    }
}
