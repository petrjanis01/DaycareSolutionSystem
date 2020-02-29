using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Address")]
    public class Address : EntityBase
    {
        public string PostCode { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public Guid? CoordinatesId { get; set; }

        public virtual Coordinates Coordinates { get; set; }
    }
}
