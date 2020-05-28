using System;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Database.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string FullName => $"{FirstName} {Surname}";

        public DateTime Birthdate { get; set; }

        public Gender Gender { get; set; }

        public virtual Picture ProfilePicture { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? ProfilePictureId { get; set; }
    }
}
