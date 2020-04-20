using System;
using DaycareSolutionSystem.Entities.Enums;

namespace DaycareSolutionSystem.Api.Host.Controllers.Employees
{
    public class EmployeeDetailDTO : EmployeeBasicDTO
    {
        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public Gender Gender { get; set; }

        public EmployeePosition EmployeePosition { get; set; }
    }
}
