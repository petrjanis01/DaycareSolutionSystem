using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Address")]
    public class Address : EntityBase
    {
        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string PostCode { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthLong)]
        public string City { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthLong)]
        public string Street { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string BuildingNumber { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string GpsCoordinates { get; set; }
    }
}
