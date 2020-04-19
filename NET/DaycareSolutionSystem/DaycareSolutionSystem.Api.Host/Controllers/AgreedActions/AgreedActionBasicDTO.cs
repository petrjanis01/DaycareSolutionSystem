using System;
using DaycareSolutionSystem.Api.Host.Controllers.Actions;

namespace DaycareSolutionSystem.Api.Host.Controllers.AgreedActions
{
    public class AgreedActionBasicDTO
    {
        public Guid? Id { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public int? EstimatedDurationMinutes { get; set; }

        public DateTime PlannedStartTime { get; set; }

        public DateTime PlannedEndTime { get; set; }

        public ActionDTO Action { get; set; }
    }
}
