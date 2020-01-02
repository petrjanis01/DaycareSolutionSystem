using System;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;

namespace DaycareSolutionSystem.Api.Host.Controllers.Schedule
{
    public class RegisteredActionDTO
    {
        public Guid Id { get; set; }

        public DateTime ActionStartedDateTime { get; set; }

        public DateTime ActionFinishedDateTime { get; set; }

        public ActionDTO Action { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public string Comment { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCanceled { get; set; }

        public PictureDTO Photo { get; set; }
    }
}
