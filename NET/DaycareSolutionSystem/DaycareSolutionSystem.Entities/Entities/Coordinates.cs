using System.ComponentModel.DataAnnotations.Schema;

namespace DaycareSolutionSystem.Database.Entities.Entities
{
    [Table("Coordinates")]
    public class Coordinates: EntityBase
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
