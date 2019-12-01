using System;
using System.Runtime.Serialization;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Database.Entities
{
    public class Person : EntityBase
    {

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Surname { get; set; }

        public string FullName => $"{FirstName} {Surname}";

        [DataMember]
        public DateTime Birthdate { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public virtual Picture ProfilePicture { get; set; }

        [DataMember]
        public Guid? ProfilePictureId { get; set; }
    }
}
