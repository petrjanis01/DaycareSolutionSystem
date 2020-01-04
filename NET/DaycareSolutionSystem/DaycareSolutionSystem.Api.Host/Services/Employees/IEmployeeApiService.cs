using System;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.Employees
{
    public interface IEmployeeApiService
    {
        Employee ChangeEmployeeProfilePicture(string pictureUri, Guid? employeeId);

        Employee GetEmployee(Guid? employeeId);

        void ChangePassword(string newPassword);
    }
}
