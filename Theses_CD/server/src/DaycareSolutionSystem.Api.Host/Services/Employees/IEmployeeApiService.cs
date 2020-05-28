using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.Employees
{
    public interface IEmployeeApiService
    {
        List<Employee> GetAllEmployees();

        List<Employee> GetAllEmployeesExceptCurrent();

        List<Employee> GetAllCaregivers();

        Employee ChangeEmployeeProfilePicture(string pictureUri, Guid? employeeId);

        Employee GetEmployee(Guid? employeeId);

        void ChangePassword(string newPassword);

        Employee CreateEmployee(Employee employee, User user);

        Employee UpdateEmployee(Employee employee);
    }
}
