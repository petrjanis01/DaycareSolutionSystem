using System;
using DaycareSolutionSystem.Api.Host.Controllers.AgreedActions;

namespace DaycareSolutionSystem.Api.Host.Controllers.IndividualPlans
{
    public class AgreedActionsForDayDTO
    {
        public DayOfWeek Day { get; set; }

        public AgreedActionBasicDTO[] AgreedActions { get; set; }
    }
}
