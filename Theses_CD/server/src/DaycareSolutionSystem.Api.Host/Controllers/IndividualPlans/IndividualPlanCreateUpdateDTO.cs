using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.IndividualPlans
{
    public class IndividualPlanCreateUpdateDTO
    {
        public Guid? Id { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidUntil { get; set; }

        public Guid ClientId { get; set; }
    }
}
