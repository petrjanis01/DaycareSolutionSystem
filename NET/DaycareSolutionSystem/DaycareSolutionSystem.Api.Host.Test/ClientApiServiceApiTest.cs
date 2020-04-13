using System;
using System.Linq;
using System.Security.Claims;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DaycareSolutionSystem.Api.Host.Test
{
    [TestClass]
    public class ClientApiServiceTest : ApiTestBase
    {
        private IClientApiService _testedService;
        private Mock<IHttpContextAccessor> _httpContextMock;

        [TestInitialize]
        public void TestInit()
        {
            _httpContextMock = new Mock<IHttpContextAccessor>();

            _testedService = new ClientApiService(DataContext, _httpContextMock.Object);
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_AllTodayAndNotStarted()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client1 = new Client();
            var client2 = new Client();
            DataContext.Clients.AddRange(new[] { client2, client1 });

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddHours(1), ClientId = client1.Id, EmployeeId = employee.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddHours(2), ClientId = client1.Id, EmployeeId = employee.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now, ClientId = client2.Id, EmployeeId = employee.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetNextNotStartedRegisteredActionsToday(employee.Id);

            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action1.Id));
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action3.Id));
            Assert.IsNull(result.FirstOrDefault(r => r.Id == action2.Id));
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_InOtherDays()
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
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action1.Id));
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_WithOtherEmployees()
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
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action1.Id));
        }

        [TestMethod]
        public void GetNextNotStartedRegisteredActionsToday_SomeStarted()
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
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action2.Id));
        }

        private Guid CreateClientEmployeePlanActionStructure(Guid employeeId, Guid clientId, bool isPlanValid)
        {
            var from = isPlanValid ? DateTime.Today.AddDays(-10) : DateTime.Today.AddDays(5);

            var plan = new IndividualPlan();
            plan.ClientId = clientId;
            plan.ValidFromDate = from;
            plan.ValidUntilDate = DateTime.Today.AddDays(10);
            DataContext.IndividualPlans.Add(plan);

            var agreedActionTomorrow = new AgreedClientAction();
            agreedActionTomorrow.IndividualPlanId = plan.Id;
            agreedActionTomorrow.EmployeeId = employeeId;
            agreedActionTomorrow.Day = DateTime.Today.DayOfWeek;
            agreedActionTomorrow.PlannedStartTime = DateTime.Now.AddHours(1).TimeOfDay;

            var agreedActionYesterday = new AgreedClientAction();
            agreedActionYesterday.IndividualPlanId = plan.Id;
            agreedActionYesterday.EmployeeId = employeeId;
            agreedActionYesterday.Day = DateTime.Today.AddDays(-1).DayOfWeek;
            agreedActionYesterday.PlannedStartTime = DateTime.Now.AddHours(1).TimeOfDay;

            DataContext.AgreedClientActions.AddRange(new[] { agreedActionTomorrow, agreedActionYesterday });

            return agreedActionTomorrow.Id;
        }

        [TestMethod]
        public void GetAllNextRegisteredActions_SingleClient_RegisteredActionsExists_ValidPlan()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client = new Client();
            DataContext.Clients.Add(client);

            CreateClientEmployeePlanActionStructure(employee.Id, client.Id, true);

            var action1 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddHours(-1), ClientId = client.Id, EmployeeId = employee.Id };
            var action2 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddHours(1), ClientId = client.Id, EmployeeId = employee.Id };
            var action3 = new RegisteredClientAction
            { PlannedStartDateTime = DateTime.Now.AddHours(2), ClientId = client.Id, EmployeeId = employee.Id };

            DataContext.RegisteredClientActions.AddRange(new[] { action1, action2, action3 });
            DataContext.SaveChanges();

            var result = _testedService.GetAllNextRegisteredActions(employee.Id);

            Assert.IsNull(result.FirstOrDefault(r => r.Id == action1.Id));
            Assert.IsNull(result.FirstOrDefault(r => r.Id == action3.Id));
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == action2.Id));
        }

        [TestMethod]
        public void GetAllNextRegisteredActions_MultipleClients_NoRegisteredActions_ValidPlans()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client = new Client();
            var client1 = new Client();
            DataContext.Clients.AddRange(new[] { client, client1 });

            var validActionId = CreateClientEmployeePlanActionStructure(employee.Id, client.Id, true);
            var validActionId1 = CreateClientEmployeePlanActionStructure(employee.Id, client1.Id, true);
            DataContext.SaveChanges();

            var result = _testedService.GetAllNextRegisteredActions(employee.Id);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(validActionId, result.FirstOrDefault(a => a.ClientId == client.Id).AgreedClientActionId);
            Assert.AreEqual(validActionId1, result.FirstOrDefault(a => a.ClientId == client1.Id).AgreedClientActionId);
        }


        [TestMethod]
        public void GetAllNextRegisteredActions_MultipleClients_NoRegisteredActions_SomePlansNotValid()
        {
            var employee = new Employee();
            DataContext.Employees.Add(employee);

            var client = new Client();
            var client1 = new Client();
            DataContext.Clients.AddRange(new[] { client, client1 });

            var validActionId = CreateClientEmployeePlanActionStructure(employee.Id, client.Id, true);
            CreateClientEmployeePlanActionStructure(employee.Id, client1.Id, false);
            DataContext.SaveChanges();

            var result = _testedService.GetAllNextRegisteredActions(employee.Id);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(validActionId, result.FirstOrDefault(a => a.ClientId == client.Id).AgreedClientActionId);
            Assert.IsNull(result.FirstOrDefault(a => a.ClientId == client1.Id));
        }
    }
}
