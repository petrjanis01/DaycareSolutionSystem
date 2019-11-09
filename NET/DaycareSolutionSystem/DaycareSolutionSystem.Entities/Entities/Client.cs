using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Client")]
    public class Client : Person
    {
        public Client()
        {
            IndividualPlans = new HashSet<IndividualPlan>();
        }

        [DataMember]
        public virtual Address Address { get; set; }

        [DataMember]
        public virtual Guid? AddressId { get; set; }

        [DataMember]
        public virtual HashSet<IndividualPlan> IndividualPlans { get; set; }
    }
}
