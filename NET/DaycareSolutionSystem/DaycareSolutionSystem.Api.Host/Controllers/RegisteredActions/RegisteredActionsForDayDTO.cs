using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.Schedule
{
    public class RegisteredActionsForDayDTO
    {
        public DateTime Date { get; set; }

        public DayOfWeek Day { get; set; }

        public RegisteredActionDetailDTO[] RegisteredActionDetails { get; set; }

        public bool ContainsLast { get; set; }
    }
}
