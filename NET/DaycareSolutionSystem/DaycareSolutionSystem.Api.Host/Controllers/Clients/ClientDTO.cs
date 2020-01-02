using System;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public PictureDTO ProfilePicture { get; set; }

        public AddressDTO Address { get; set; }

    }
}
