using System;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.Employees
{
    public class EmployeeApiService : ApiServiceBase, IEmployeeApiService
    {
        public EmployeeApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
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
    }
}
