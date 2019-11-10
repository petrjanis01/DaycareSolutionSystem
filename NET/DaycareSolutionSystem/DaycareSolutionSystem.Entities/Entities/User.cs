using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("User")]
    public class User : EntityBase
    {
        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string LoginName { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string Password { get; set; }

        [DataMember]
        public Guid EmployeeId { get; set; }

        [DataMember]
        public virtual Employee Employee { get; set; }
    }
}
