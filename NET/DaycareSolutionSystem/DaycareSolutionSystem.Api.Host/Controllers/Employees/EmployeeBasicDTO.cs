using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.Employees
{
    public class EmployeeBasicDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string ProfilePictureUri { get; set; }
    }
}
