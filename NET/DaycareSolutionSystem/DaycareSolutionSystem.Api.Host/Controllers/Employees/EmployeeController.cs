using System;
using System.Collections.Generic;
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



        [HttpGet]
        [Authorize(Roles = "Manager")]
        public EmployeeBasicDTO[] GetAllEmployeesBasicsExceptCurrent()
        {
            var employees = _employeeApiService.GetAllEmployeesExceptCurrent();

            var dtos = new List<EmployeeBasicDTO>();

            foreach (var employee in employees)
            {
                var dto = MapEmployeeToBasicDto(employee);
                dtos.Add(dto);
            }

            return dtos.ToArray();
        }

        [HttpGet]
        [Route("all-caregivers")]
        [Authorize(Roles = "Manager")]
        public EmployeeBasicDTO[] GetAllCareGiversBasics()
        {
            var employees = _employeeApiService.GetAllCaregivers();

            var dtos = new List<EmployeeBasicDTO>();

            foreach (var employee in employees)
            {
                var dto = MapEmployeeToBasicDto(employee);
                dtos.Add(dto);
            }

            return dtos.ToArray();
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
                var dto = MapEmployeeToDetailDto(employee);

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

        // mappers
        private EmployeeDetailDTO MapEmployeeToDetailDto(Employee employee)
        {
            var dto = new EmployeeDetailDTO();
            dto.Id = employee.Id;
            dto.FullName = employee.FullName;
            dto.Birthdate = employee.Birthdate;
            dto.EmployeePosition = employee.EmployeePosition;
            dto.ProfilePictureUri = FormatPictureToBase64(employee.ProfilePicture);
            dto.PhoneNumber = employee.PhoneNumber;
            dto.Email = employee.Email;

            return dto;
        }

        private EmployeeBasicDTO MapEmployeeToBasicDto(Employee employee)
        {
            var dto = new EmployeeBasicDTO();
            dto.Id = employee.Id;
            dto.FullName = employee.FullName;
            dto.ProfilePictureUri = FormatPictureToBase64(employee.ProfilePicture);

            return dto;
        }
    }
}