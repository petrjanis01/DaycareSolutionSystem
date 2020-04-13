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

        public void GenerateNextMonthRegisteredActions()
        {
            var fromDate = DateTime.Today;
            var untilDate = DateTime.Today.AddMonths(1);

            var agreedActions = GetValidAgreedActions(fromDate).ToList();
            var oldRegisteredActions = DataContext.RegisteredClientActions.ToList();
            var newRegisteredActions = new List<RegisteredClientAction>();

            while (fromDate <= untilDate)
            {
                // add actions from newly valid plan to collection
                var oldIds = agreedActions.Select(ac => ac.Id);
                var newAgreedActions = GetValidAgreedActions(fromDate)
                    // only that day
                    .Where(ac => ac.Day == fromDate.DayOfWeek)
                    .Where(ac => oldIds.Contains(ac.Id) == false);

                agreedActions.AddRange(newAgreedActions);

                var actionsInDay = agreedActions.Where(ac => ac.Day == fromDate.DayOfWeek).ToList();

                foreach (var action in actionsInDay)
                {
                    // when registered action hasn't been created from agreed action
                    if (oldRegisteredActions.FirstOrDefault(ac => ac.AgreedClientActionId == action.Id) != null)
                    {
                        var newAction = CreateRegisteredActionFromAgreedAction(action, fromDate);
                        newRegisteredActions.Add(newAction);
                    }
                }

                fromDate = fromDate.AddDays(1);
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

        private IOrderedQueryable<AgreedClientAction> GetValidAgreedActions(DateTime date)
        {
            var agreedActions = DataContext.AgreedClientActions
                .Where(ac => ac.IndividualPlan.ValidFromDate <= date && ac.IndividualPlan.ValidUntilDate >= date)
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
                actionDo.Action = registeredAction.AgreedClientAction.Action;
                actionDo.RegisteredClientAction = registeredAction;
                actionDo.IsLast = false;

                registeredActionsPerDay[registeredAction.PlannedStartDateTime.Date].Add(actionDo);
            }

            EnsureLastIsMarked(registeredActionsPerDay);

            return registeredActionsPerDay;
        }

        public RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto)
        {
            var registeredAction = DataContext.RegisteredClientActions.Find(dto.Id);

            registeredAction.Comment = dto.Comment;
            registeredAction.ActionStartedDateTime = dto.ActionStartedDateTime;

            if (dto.ActionFinishedDateTime.HasValue)
            {
                registeredAction.ActionFinishedDateTime = dto.ActionFinishedDateTime;
                registeredAction.IsCompleted = true;
                dto.IsCompleted = true;
            }

            if (dto.Photo != null && string.IsNullOrEmpty(dto.Photo.PictureUri) == false)
            {
                var picture = Base64ImageHelper.CreatePictureFromUri(dto.Photo.PictureUri);
                if (registeredAction.Photo != null)
                {
                    registeredAction.Photo.MimeType = picture.MimeType;
                    registeredAction.Photo.BinaryData = picture.BinaryData;
                }
                else
                {
                    registeredAction.Photo = picture;
                    DataContext.Pictures.Add(picture);
                }
            }

            registeredAction.IsCanceled = dto.IsCanceled;

            DataContext.SaveChanges();

            return dto;
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
