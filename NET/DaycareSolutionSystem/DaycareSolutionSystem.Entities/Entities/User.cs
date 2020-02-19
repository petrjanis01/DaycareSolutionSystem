using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("User")]
    public class User : EntityBase
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
