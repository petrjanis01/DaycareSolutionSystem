using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class ClientWithNextActionDTO
    {
        public Guid ClientId { get; set; }

        public RegisteredActionBasicDTO NextAction { get; set; }
    }
}
