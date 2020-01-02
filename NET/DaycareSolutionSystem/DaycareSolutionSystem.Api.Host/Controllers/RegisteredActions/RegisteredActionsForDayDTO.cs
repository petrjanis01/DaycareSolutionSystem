using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaycareSolutionSystem.Api.Host.Controllers.RegisteredActions;

namespace DaycareSolutionSystem.Api.Host.Controllers.Schedule
{
    public class RegisteredActionsForDayDTO
    {
        public DateTime Date { get; set; }

        public DayOfWeek Day { get; set; }

        public RegisteredActionsClientDTO[] RegisteredActionsClient { get; set; }

        public bool ContainsLast { get; set; }
    }
}
