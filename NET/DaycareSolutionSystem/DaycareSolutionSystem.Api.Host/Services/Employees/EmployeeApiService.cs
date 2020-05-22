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

        public Employee UpdateEmployee(Employee employee)
        {
            var queriedEmployee = DataContext.Employees.Find(employee.Id);
            queriedEmployee.EmployeePosition = employee.EmployeePosition;
            queriedEmployee.FirstName = employee.FirstName;
            queriedEmployee.Surname = employee.Surname;
            queriedEmployee.Gender = employee.Gender;
            queriedEmployee.Birthdate = employee.Birthdate;
            queriedEmployee.Email = employee.Email;
            queriedEmployee.PhoneNumber = employee.PhoneNumber;

            if (queriedEmployee.ProfilePicture == null && employee.ProfilePicture != null)
            {
                DataContext.Pictures.Add(employee.ProfilePicture);
                queriedEmployee.ProfilePicture = employee.ProfilePicture;
                queriedEmployee.ProfilePictureId = employee.ProfilePictureId;

            }
            else if (employee.ProfilePicture != null)
            {
                queriedEmployee.ProfilePicture.MimeType = employee.ProfilePicture.MimeType;
                queriedEmployee.ProfilePicture.BinaryData = employee.ProfilePicture.BinaryData;
            }

            DataContext.SaveChanges();

            return queriedEmployee;
        }

        public Employee CreateEmployee(Employee employee, User user)
        {
            var hashedPassword = PasswordHasher.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.EmployeeId = employee.Id;

            DataContext.Users.Add(user);
            DataContext.Employees.Add(employee);
            DataContext.SaveChanges();

            return employee;
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
            user.Password = PasswordHasher.HashPassword(newPassword);
            DataContext.SaveChanges();
        }
    }
}
