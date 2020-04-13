using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.Actions
{
    public class ActionDTO
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string GeneralDescription { get; set; }
    }
}
