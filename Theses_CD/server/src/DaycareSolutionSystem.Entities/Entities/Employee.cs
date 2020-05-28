using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public EmployeePosition EmployeePosition { get; set; }

        public virtual HashSet<AgreedClientAction> AgreedClientActions { get; set; }
    }
}
