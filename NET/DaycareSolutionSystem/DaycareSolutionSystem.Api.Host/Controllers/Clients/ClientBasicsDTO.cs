using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class ClientBasicsDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public PictureDTO ProfilePicture { get; set; }
    }
}
