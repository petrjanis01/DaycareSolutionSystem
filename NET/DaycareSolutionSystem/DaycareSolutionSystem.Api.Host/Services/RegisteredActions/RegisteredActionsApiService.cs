using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.AspNetCore.Http;

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
            var startDate = lastActionDisplayedId.HasValue
                ? (DataContext.RegisteredClientActions.Find(lastActionDisplayedId)).ActionStartedDateTime
                : DateTime.Today;

            var registeredActions = DataContext.RegisteredClientActions
                .Where(rca => rca.EmployeeId == GetCurrentUser().EmployeeId
                              && rca.ActionStartedDateTime > startDate)
                .OrderBy(rca => rca.ActionStartedDateTime)
                .Take(count)
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
            if (registeredActionsPerDay.Any() == false)
            {
                return;
            }

            var actionsForLastDay = registeredActionsPerDay[registeredActionsPerDay.Keys.Max()];

            // last action from list
            var lastAction = actionsForLastDay[^1];

            var nextExist = DataContext.RegisteredClientActions.Any(rca =>
                rca.ActionStartedDateTime > lastAction.RegisteredClientAction.ActionStartedDateTime);

            lastAction.IsLast = nextExist == false;
        }
    }
}
