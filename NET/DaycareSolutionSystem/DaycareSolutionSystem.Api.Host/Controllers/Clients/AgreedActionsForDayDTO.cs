using System;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class AgreedActionsForDayDTO
    {
        public DayOfWeek Day { get; set; }

        public AgreedActionDTO[] AgreedActions { get; set; }
    }
}
