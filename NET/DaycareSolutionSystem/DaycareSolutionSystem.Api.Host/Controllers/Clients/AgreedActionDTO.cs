using System;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class AgreedActionDTO
    {
        public Guid Id { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public int EstimatedDurationMinutes { get; set; }

        public TimeSpan PlannedStartTime { get; set; }

        public TimeSpan PlannedEndTime { get; set; }

        public ActionDTO Action { get; set; }
    }
}
