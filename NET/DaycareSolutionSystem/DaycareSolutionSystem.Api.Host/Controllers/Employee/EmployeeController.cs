using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DaycareSolutionSystem.Api.Host.Services;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : DssBaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DssDataContext DataContext;

        public EmployeeController(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            DataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("get-employee-detail")]
        public IActionResult GetEmployeeDetail(Guid? employeeId = null)
        {
            var employee = employeeId.HasValue ? DataContext.Employees.Find(employeeId) : GetCurrentUser()?.Employee;

            if (employee != null)
            {
                var dto = new EmployeeDetailDTO();
                dto.FullName = employee.FullName;
                dto.Birthdate = employee.Birthdate;
                dto.EmployeePosition = employee.EmployeePosition;
                dto.ProfilePictureUri = FormatPictureToBase64(employee.ProfilePicture);

                return Ok(dto);
            }

            return NotFound("Employee not found.");
        }

        [HttpPost]
        [Route("change-profile-picture")]
        public IActionResult ChangeEmployeeProfilePicture(string pictureUri, Guid? employeeId = null)
        {
            var employee = employeeId.HasValue ? DataContext.Employees.Find(employeeId) : GetCurrentUser()?.Employee;

            if (employee != null)
            {
                return Ok();
            }

            return NotFound("Employee not found.");
        }


        protected User GetCurrentUser()
        {
            var loginName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User currentUser;

            if (string.IsNullOrEmpty(loginName)
                || (currentUser = DataContext.Users.FirstOrDefault(u => u.LoginName == loginName)) == null)
            {
                return null;
            }

            return currentUser;
        }
    }
}