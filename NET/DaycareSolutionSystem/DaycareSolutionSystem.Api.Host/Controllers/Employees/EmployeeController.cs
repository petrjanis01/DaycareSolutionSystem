using System;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Api.Host.Services.Employees;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : DssBaseController
    {
        private readonly IEmployeeApiService _employeeApiService;

        public EmployeeController(IEmployeeApiService employeeApiService)
        {
            _employeeApiService = employeeApiService;
        }

        [HttpPut]
        [Route("change-password")]
        public void ChangePassword(string newPassword)
        {
            _employeeApiService.ChangePassword(newPassword);
        }

        [HttpGet]
        [Route("get-employee-detail")]
        public EmployeeDetailDTO GetEmployeeDetail(Guid? employeeId = null)
        {
            var employee = _employeeApiService.GetEmployee(employeeId);

            if (employee != null)
            {
                var dto = MapEmployeeToDto(employee);

                return dto;
            }

            return null;
        }

        [HttpPost]
        [Route("change-profile-picture")]
        public IActionResult ChangeEmployeeProfilePicture(PictureDTO dto, Guid? employeeId = null)
        {
            if (dto != null)
            {
                var employee = _employeeApiService.ChangeEmployeeProfilePicture(dto.PictureUri, employeeId);

                if (employee != null)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        private EmployeeDetailDTO MapEmployeeToDto(Employee employee)
        {
            var dto = new EmployeeDetailDTO();
            dto.FullName = employee.FullName;
            dto.Birthdate = employee.Birthdate;
            dto.EmployeePosition = employee.EmployeePosition;
            dto.ProfilePictureUri = FormatPictureToBase64(employee.ProfilePicture);
            dto.PhoneNumber = employee.PhoneNumber;
            dto.Email = employee.Email;

            return dto;
        }
    }
}