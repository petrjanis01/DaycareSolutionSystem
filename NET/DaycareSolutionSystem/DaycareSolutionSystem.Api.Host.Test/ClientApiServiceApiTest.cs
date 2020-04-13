using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaycareSolutionSystem.Api.Host.Test
{
    [TestClass]
    public class ClientApiServiceTest : ApiTestBase
    {
        private IClientApiService _testedService;

        [TestInitialize]
        public void TestInit()
        {
            _testedService = new ClientApiService(DataContext, new HttpContextAccessor());
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_AllTodayAndNotStarted_CorrectDataReturned()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client1 = new Client();
            var client2 = new Client();
            DataContext.Clients.AddRange(new[] { client2, client1 });

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client1.Id, EmployeeId = employee.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client1.Id, EmployeeId = employee.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client2.Id, EmployeeId = employee.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetNextNotStartedRegisteredActionsToday(employee.Id);

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_InOtherDays_ReturnsCorrectAmount()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client1 = new Client();
            var client2 = new Client();
            DataContext.Clients.AddRange(new[] { client2, client1 });

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client1.Id, EmployeeId = employee.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddDays(1), ClientId = client1.Id, EmployeeId = employee.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddDays(-1), ClientId = client2.Id, EmployeeId = employee.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetNextNotStartedRegisteredActionsToday(employee.Id);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_WithOtherEmployees_ReturnsCorrectAmount()
        {
            var employee1 = new Employee();
            var employee2 = new Employee();
            DataContext.Employees.AddRange(new[] { employee1, employee2 });

            var client = new Client();
            DataContext.Clients.AddRange(new[] { client });

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client.Id, EmployeeId = employee1.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client.Id, EmployeeId = employee2.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client.Id, EmployeeId = employee2.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetNextNotStartedRegisteredActionsToday(employee1.Id);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_SomeStarted_ReturnsCorrectAmount()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client = new Client();
            DataContext.Clients.AddRange(new[] { client });

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ActionStartedDateTime = DateTime.Now.AddHours(-1), ClientId = client.Id, EmployeeId = employee.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client.Id, EmployeeId = employee.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ActionStartedDateTime = DateTime.Now.AddHours(-3), ClientId = client.Id, EmployeeId = employee.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetNextNotStartedRegisteredActionsToday(employee.Id);

            Assert.AreEqual(1, result.Count);
        }
    }
}
