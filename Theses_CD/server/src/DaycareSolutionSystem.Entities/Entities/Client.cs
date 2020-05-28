using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Client")]
    public class Client : Person
    {
        public Client()
        {
            IndividualPlans = new HashSet<IndividualPlan>();
        }

        public virtual Address Address { get; set; }

        public Guid AddressId { get; set; }

        public virtual HashSet<IndividualPlan> IndividualPlans { get; set; }
    }
}
