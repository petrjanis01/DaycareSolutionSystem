using System;

namespace DaycareSolutionSystem.Api.Host.Controllers.AgreedActions
{
    public class AgreedActionDto : AgreedActionBasicDTO
    {
        public Guid EmployeeId { get; set; }

        public Guid IndividualPlanId { get; set; }

        public DayOfWeek Day { get; set; }

    }
}
