using System;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Api.Host.Controllers.Employees
{
    public class EmployeeDetailDTO
    {
        public string FullName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePictureUri { get; set; }

        public EmployeePosition EmployeePosition { get; set; }
    }
}
