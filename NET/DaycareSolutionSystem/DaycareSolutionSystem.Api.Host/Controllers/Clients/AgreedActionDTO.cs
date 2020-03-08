using System;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class AgreedActionDTO
    {
        public Guid Id { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public int EstimatedDurationMinutes { get; set; }

        public DateTime PlannedStartTime { get; set; }

        public DateTime PlannedEndTime { get; set; }

        public ActionDTO Action { get; set; }
    }
}
