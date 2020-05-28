using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    public class RegisteredActionBasicDTO
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public string Name { get; set; }
    }
}
