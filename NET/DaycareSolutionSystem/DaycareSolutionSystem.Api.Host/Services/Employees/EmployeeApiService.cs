using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Entities.Enums;
using DaycareSolutionSystem.Helpers;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.Employees
{
    public class EmployeeApiService : ApiServiceBase, IEmployeeApiService
    {
        public EmployeeApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
        }

        public List<Employee> GetAllEmployeesExceptCurrent()
        {
            var currentEmployeeId = GetCurrentUser().EmployeeId;

            var employees = DataContext.Employees.Where(e => e.Id != currentEmployeeId).ToList();
            return employees;
        }

        public List<Employee> GetAllCaregivers()
        {
            var caregivers = DataContext.Employees.Where(e => e.EmployeePosition == EmployeePosition.Caregiver)
                .ToList();

            return caregivers;
        }

        public Employee ChangeEmployeeProfilePicture(string pictureUri, Guid? employeeId)
        {
            employeeId ??= GetCurrentUser()?.Employee.Id;

            return ChangeProfilePicture<Employee>(employeeId, pictureUri);
        }

        public Employee GetEmployee(Guid? employeeId)
        {
            var employee = employeeId.HasValue ? DataContext.Employees.Find(employeeId) : GetCurrentUser()?.Employee;

            return employee;
        }

        public void ChangePassword(string newPassword)
        {
            var userId = GetCurrentUser().Id;

            var user = DataContext.Users.Find(userId);
            user.Password = new PasswordHasher().HashPassword(newPassword);
            DataContext.SaveChanges();
        }
    }
}
