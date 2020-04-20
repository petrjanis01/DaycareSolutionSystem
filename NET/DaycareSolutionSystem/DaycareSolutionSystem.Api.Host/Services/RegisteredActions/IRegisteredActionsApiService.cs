using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public interface IRegisteredActionsApiService
    {
        Dictionary<DateTime, List<RegisteredActionDO>> GetRegisteredActionsPerDay(int count, DateTime date, Guid? lastActionDisplayedId);

        RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto);

        void GenerateRegisteredActionsForPeriod(DateTime? fromDate = null, DateTime? untilDate = null);

        public List<RegisteredClientAction> GetAllRegisteredClientActionsInGivenMonth(DateTime date, Guid? clientId = null, Guid? employeeId = null);
    }
}
