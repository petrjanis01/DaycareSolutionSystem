using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Helpers;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public class RegisteredActionsApiService : ApiServiceBase, IRegisteredActionsApiService
    {
        public RegisteredActionsApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
        }

        public List<RegisteredClientAction> GetAllRegisteredClientActionsInGivenMonth(DateTime date, Guid? clientId = null, Guid? employeeId = null)
        {
            var from = new DateTime(date.Year, date.Month, 1);
            var until = from.AddMonths(1).AddDays(-1);

            var registeredActions = DataContext.RegisteredClientActions.Where(ra =>
                ra.PlannedStartDateTime.Date >= from.Date
                && ra.PlannedStartDateTime.Date <= until.Date);

            if (clientId.HasValue)
            {
                registeredActions = registeredActions.Where(ra => ra.ClientId == clientId.Value);
            }

            if (employeeId.HasValue)
            {
                registeredActions = registeredActions.Where(ra => ra.EmployeeId == employeeId.Value);
            }

            return registeredActions.ToList();
        }

        public void GenerateRegisteredActionsForPeriod(DateTime? fromDate = null, DateTime? untilDate = null)
        {
            fromDate = fromDate.HasValue ? fromDate : DateTime.Today;
            untilDate = untilDate.HasValue ? untilDate : DateTime.Today.AddMonths(1);

            var agreedActions = GetValidAgreedActions(fromDate.Value).ToList();
            var oldRegisteredActions = DataContext.RegisteredClientActions.ToList();
            var newRegisteredActions = new List<RegisteredClientAction>();

            while (fromDate <= untilDate)
            {
                // add actions from newly valid plan to collection
                var oldIds = agreedActions.Select(ac => ac.Id);
                var newAgreedActions = GetValidAgreedActions(fromDate.Value)
                    // only that day
                    .Where(ac => ac.Day == fromDate.Value.DayOfWeek)
                    .Where(ac => oldIds.Contains(ac.Id) == false);

                agreedActions.AddRange(newAgreedActions);

                var actionsInDay = agreedActions.Where(ac => ac.Day == fromDate.Value.DayOfWeek).ToList();

                foreach (var action in actionsInDay)
                {
                    // when registered action hasn't been created from agreed action
                    if (oldRegisteredActions.Where(ac => ac.PlannedStartDateTime.Date == fromDate.Value.Date)
                            .FirstOrDefault(ac => ac.AgreedClientActionId == action.Id) != null)
                    {
                        var newAction = CreateRegisteredActionFromAgreedAction(action, fromDate.Value);
                        newRegisteredActions.Add(newAction);
                    }
                }

                fromDate = fromDate.Value.AddDays(1);
            }

            DataContext.RegisteredClientActions.AddRange(newRegisteredActions);
        }

        private RegisteredClientAction CreateRegisteredActionFromAgreedAction(AgreedClientAction agreedClientAction, DateTime date)
        {
            var registeredAction = new RegisteredClientAction();

            registeredAction.IsCanceled = false;
            registeredAction.ClientId = agreedClientAction.IndividualPlan.ClientId;
            registeredAction.PlannedStartDateTime = date.Add(agreedClientAction.PlannedStartTime);
            registeredAction.EmployeeId = agreedClientAction.EmployeeId;
            registeredAction.AgreedClientActionId = agreedClientAction.Id;
            registeredAction.IsCompleted = false;

            return registeredAction;
        }

        // gets agreed actions that are valid and has valid individual plan
        private IOrderedQueryable<AgreedClientAction> GetValidAgreedActions(DateTime date)
        {
            var agreedActions = DataContext.AgreedClientActions
                .Where(ac => ac.IndividualPlan.ValidFromDate <= date && ac.IndividualPlan.ValidUntilDate >= date)
                .Where(ac => ac.IsValid)
                .OrderBy(ac => ac.Day);

            return agreedActions;
        }

        // TODO unit test this
        public Dictionary<DateTime, List<RegisteredActionDO>> GetRegisteredActionsPerDay(int count, DateTime date, Guid? lastActionDisplayedId)
        {
            var startDate = lastActionDisplayedId.HasValue
                ? (DataContext.RegisteredClientActions.Find(lastActionDisplayedId)).PlannedStartDateTime
                : date.Date;

            var registeredActions = DataContext.RegisteredClientActions
                .Where(rca => rca.EmployeeId == GetCurrentUser().EmployeeId
                              && rca.PlannedStartDateTime > startDate)
                .OrderBy(rca => rca.PlannedStartDateTime)
                .Take(count)
                .ToList();

            var registeredActionsPerDay = new Dictionary<DateTime, List<RegisteredActionDO>>();

            foreach (var registeredAction in registeredActions)
            {
                if (registeredActionsPerDay.ContainsKey(registeredAction.PlannedStartDateTime.Date) == false)
                {
                    registeredActionsPerDay.Add(registeredAction.PlannedStartDateTime.Date, new List<RegisteredActionDO>());
                }

                var actionDo = new RegisteredActionDO();
                actionDo.Client = registeredAction.Client;
                actionDo.Action = registeredAction.Action;
                actionDo.RegisteredClientAction = registeredAction;
                actionDo.IsLast = false;

                registeredActionsPerDay[registeredAction.PlannedStartDateTime.Date].Add(actionDo);
            }

            EnsureLastIsMarked(registeredActionsPerDay);

            return registeredActionsPerDay;
        }

        public void CreateRegisteredAction(RegisteredClientAction action)
        {
            // to simplify logic adhoc actions always have 1 hour duration
            action.EstimatedDurationMinutes = 60;
            DataContext.RegisteredClientActions.Add(action);
            DataContext.SaveChanges();
        }

        public RegisteredClientAction UpdateRegisteredAction(RegisteredClientAction action)
        {
            var queriedAction = DataContext.RegisteredClientActions.Find(action.Id);

            queriedAction.Comment = action.Comment;
            queriedAction.ActionStartedDateTime = action.ActionStartedDateTime;
            queriedAction.PlannedStartDateTime = action.PlannedStartDateTime;
            queriedAction.EmployeeId = action.EmployeeId;
            queriedAction.ActionId = action.ActionId;


            if (action.ActionFinishedDateTime.HasValue)
            {
                queriedAction.ActionFinishedDateTime = action.ActionFinishedDateTime;
                queriedAction.IsCompleted = true;
            }

            if (action.Photo != null)
            {
                if (queriedAction.Photo != null)
                {
                    queriedAction.Photo.MimeType = action.Photo.MimeType;
                    queriedAction.Photo.BinaryData = action.Photo.BinaryData;
                }
                else
                {
                    queriedAction.Photo = action.Photo;
                    DataContext.Pictures.Add(action.Photo);
                }
            }

            if (queriedAction.AgreedClientActionId.HasValue == false)
            {
                queriedAction.EstimatedDurationMinutes = action.EstimatedDurationMinutes;
            }

            queriedAction.IsCanceled = action.IsCanceled;

            DataContext.SaveChanges();

            return queriedAction;
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
                rca.PlannedStartDateTime > lastAction.RegisteredClientAction.PlannedStartDateTime);

            lastAction.IsLast = nextExist == false;
        }
    }
}
