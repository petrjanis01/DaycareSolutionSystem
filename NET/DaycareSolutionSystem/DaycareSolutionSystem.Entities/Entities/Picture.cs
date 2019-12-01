using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    public class Picture : EntityBase
    {
        [DataMember]
        public byte[] BinaryData { get; set; }

        [DataMember]
        [MaxLength(StringMaxLengthConstants.StringLengthShort)]
        public string MimeType { get; set; }
    }
}
