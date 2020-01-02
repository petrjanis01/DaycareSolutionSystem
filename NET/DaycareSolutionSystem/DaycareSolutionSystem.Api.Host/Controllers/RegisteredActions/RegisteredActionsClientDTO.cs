using System;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;

namespace DaycareSolutionSystem.Api.Host.Controllers.RegisteredActions
{
    public class RegisteredActionsClientDTO
    {
        public Guid ClientId { get; set; }

        public RegisteredActionDTO[] RegisteredActions { get; set; }
    }
}
