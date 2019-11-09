using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Employee")]
    public class Employee : Person
    {
        public Employee()
        {
            AgreedClientActions = new HashSet<AgreedClientAction>();
        }

        [DataMember]
        public EmployeePosition EmployeePosition { get; set; }

        [DataMember]
        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }
    }
}
