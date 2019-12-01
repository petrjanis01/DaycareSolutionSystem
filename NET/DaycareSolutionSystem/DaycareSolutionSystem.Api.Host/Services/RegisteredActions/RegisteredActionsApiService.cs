using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public class RegisteredActionsApiService : ApiServiceBase, IRegisteredActionsApiService
    {
        public RegisteredActionsApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
        }

        public Dictionary<DateTime, List<RegisteredActionDO>> GetRegisteredActionsPerDay(int count, Guid? lastActionDisplayedId)
        {

            RegisteredClientAction lastActionDisplayed;
            var startDate = lastActionDisplayedId.HasValue
                ? (lastActionDisplayed = _dataContext.RegisteredClientActions.Find(lastActionDisplayedId)).ActionStartedDateTime
                : DateTime.Today;

            var registeredActions = _dataContext.RegisteredClientActions
                .Where(rca => rca.EmployeeId == GetCurrentUser().EmployeeId
                              && rca.ActionStartedDateTime > startDate)
                .Take(count)
                .OrderBy(rca => rca.ActionStartedDateTime)
                .ToList();

            var registeredActionsPerDay = new Dictionary<DateTime, List<RegisteredActionDO>>();

            foreach (var registeredAction in registeredActions)
            {
                if (registeredActionsPerDay.ContainsKey(registeredAction.ActionStartedDateTime.Date) == false)
                {
                    registeredActionsPerDay.Add(registeredAction.ActionStartedDateTime.Date, new List<RegisteredActionDO>());
                }

                var actionDo = new RegisteredActionDO();
                actionDo.Client = registeredAction.Client;
                actionDo.Action = registeredAction.AgreedClientAction.Action;
                actionDo.RegisteredClientAction = registeredAction;
                actionDo.IsLast = false;

                registeredActionsPerDay[registeredAction.ActionStartedDateTime.Date].Add(actionDo);
            }

            EnsureLastIsMarked(registeredActionsPerDay);

            return registeredActionsPerDay;
        }

        private void EnsureLastIsMarked(Dictionary<DateTime, List<RegisteredActionDO>> registeredActionsPerDay)
        {
            var actionsForLastDay = registeredActionsPerDay[registeredActionsPerDay.Keys.Max()];

            // last action from list
            var lastAction = actionsForLastDay[^1];

            var nextExist = _dataContext.RegisteredClientActions.Any(rca =>
                rca.ActionStartedDateTime > lastAction.RegisteredClientAction.ActionStartedDateTime);

            lastAction.IsLast = nextExist == false;
        }
    }
}
