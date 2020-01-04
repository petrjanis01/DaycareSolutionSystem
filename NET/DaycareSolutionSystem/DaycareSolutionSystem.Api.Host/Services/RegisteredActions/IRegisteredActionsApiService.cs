using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public interface IRegisteredActionsApiService
    {
        Dictionary<DateTime, List<RegisteredActionDO>> GetRegisteredActionsPerDay(int count, Guid? lastActionDisplayedId);

        RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto);

        void GenerateNextMonthRegisteredActions();
    }
}
