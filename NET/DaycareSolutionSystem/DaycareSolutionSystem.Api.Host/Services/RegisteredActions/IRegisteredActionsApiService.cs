using System;
using System.Collections.Generic;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public interface IRegisteredActionsApiService
    {
        Dictionary<DateTime, List<RegisteredActionDO>> GetRegisteredActionsPerDay(int count, Guid? lastActionDisplayedId);
    }
}
