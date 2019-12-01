using System;

namespace DaycareSolutionSystem.Api.Host.Controllers
{
    public class ClientBasicInfoDTO
    {
        public Guid Id { get; set; }

        public string ClientFullName { get; set; }

        public string ProfilePictureUri { get; set; }
    }
}
