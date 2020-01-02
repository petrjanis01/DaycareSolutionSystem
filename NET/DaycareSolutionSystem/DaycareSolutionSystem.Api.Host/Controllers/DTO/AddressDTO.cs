using System;

namespace DaycareSolutionSystem.Api.Host.Controllers.DTO
{
    public class AddressDTO
    {
        public Guid Id { get; set; }

        public string PostCode { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string GpsCoordinates { get; set; }
    }
}
