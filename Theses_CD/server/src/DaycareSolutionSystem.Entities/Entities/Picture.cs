using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Picture")]
    public class Picture : EntityBase
    {
        public byte[] BinaryData { get; set; }

        public string MimeType { get; set; }
    }
}
