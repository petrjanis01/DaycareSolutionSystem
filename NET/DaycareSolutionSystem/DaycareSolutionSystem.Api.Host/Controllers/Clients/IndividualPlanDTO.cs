using System;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class IndividualPlanDTO
    {
        public Guid Id { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidUntil { get; set; }

        public AgreedActionsForDayDTO[] ActionsForDay { get; set; }
    }
}
