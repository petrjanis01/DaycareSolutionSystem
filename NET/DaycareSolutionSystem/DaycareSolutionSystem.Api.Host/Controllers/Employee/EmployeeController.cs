using System;
using System.Linq;
using System.Security.Claims;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
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
        public EmployeeDetailDTO GetEmployeeDetail(Guid? employeeId = null)
        {
            var employee = employeeId.HasValue ? DataContext.Employees.Find(employeeId) : GetCurrentUser()?.Employee;

            if (employee != null)
            {
                var dto = new EmployeeDetailDTO();
                dto.FullName = employee.FullName;
                dto.Birthdate = employee.Birthdate;
                dto.EmployeePosition = employee.EmployeePosition;
                dto.ProfilePictureUri = FormatPictureToBase64(employee.ProfilePicture);

                return dto;
            }

            return null;
        }

        [HttpPost]
        [Route("change-profile-picture")]
        public IActionResult ChangeEmployeeProfilePicture(PictureDTO dto, Guid? employeeId = null)
        {
            var employee = employeeId.HasValue ? DataContext.Employees.Find(employeeId) : GetCurrentUser()?.Employee;

            if (employee != null && dto != null)
            {
                var picture = CreatePictureFromUri(dto.PictureUri);

                if (employee.ProfilePicture != null)
                {
                    employee.ProfilePicture.MimeType = picture.MimeType;
                    employee.ProfilePicture.BinaryData = picture.BinaryData;
                }
                else
                {
                    employee.ProfilePicture = picture;
                    DataContext.Pictures.Add(picture);
                }

                DataContext.SaveChanges();

                return Ok();
            }

            return BadRequest();
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