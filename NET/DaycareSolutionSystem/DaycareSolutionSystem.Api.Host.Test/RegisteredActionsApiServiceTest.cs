using System;
using System.Collections.Generic;
using System.Text;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Api.Host.Services.RegisteredActions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DaycareSolutionSystem.Api.Host.Test
{
    [TestClass]
    public class RegisteredActionsApiServiceTest : ApiTestBase
    {
        private IRegisteredActionsApiService _testedService;
        private Mock<IHttpContextAccessor> _httpContextMock;

        [TestInitialize]
        public void TestInit()
        {
            _httpContextMock = new Mock<IHttpContextAccessor>();

            _testedService = new RegisteredActionsApiService(DataContext, _httpContextMock.Object);
        }

        [TestMethod]
        public void GenerateNextMonthRegisteredActions()
        {
            // ....
        }

        [TestMethod]
        public void GetRegisteredActionsPerDay()
        {
            // ...
        }

    }
}
