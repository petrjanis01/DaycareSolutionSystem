using System;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Entities.Enums;
using DaycareSolutionSystem.Services;

namespace DaycareSolutionSystem.Database.Migrator
{
    public class DemoDataInitializer
    {
        private DssDataContext dataContext;
        private static PasswordHashService _passwordHashService;

        private Guid _dcEmployeeId;

        public DemoDataInitializer(DssDataContext dc)
        {
            dataContext = dc;
            _passwordHashService = new PasswordHashService();
        }

        public void CreateDemoData()
        {
            CreateEmployees();
            CreateUserAccounts();
        }

        private void CreateEmployees()
        {
            var dcEmployee = new Employee();
            dcEmployee.Gender = Gender.Male;
            dcEmployee.FirstName = "Daycare";
            dcEmployee.Surname = "Employee";
            dcEmployee.Birthdate = new DateTime(1990, 6, 2);
            dcEmployee.EmployeePosition = EmployeePosition.Caregiver;

            var emp = new Employee();
            emp.Gender = Gender.Female;
            emp.FirstName = "1";
            emp.Surname = "aaa";
            emp.Birthdate = new DateTime(2000,1,1);
            emp.EmployeePosition = EmployeePosition.Manager;

            _dcEmployeeId = dcEmployee.Id;

            dataContext.Employees.AddRange(new[]
            {
                dcEmployee,
                emp
            });

            dataContext.SaveChanges();
        }

        private void CreateUserAccounts()
        {
            var user = new User();
            user.EmployeeId = _dcEmployeeId;
            user.LoginName = "dcemp";
            user.Password = _passwordHashService.HashPassword("1234");


            dataContext.Users.AddRange(new[]
            {
                user,
            });

            dataContext.SaveChanges();
        }
    }
}
