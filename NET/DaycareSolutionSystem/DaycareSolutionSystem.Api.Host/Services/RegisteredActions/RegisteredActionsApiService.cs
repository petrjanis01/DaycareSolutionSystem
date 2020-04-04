using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
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

        public void GenerateNextMonthRegisteredActions()
        {
            var lastGeneratedAction = DataContext.RegisteredClientActions
                .OrderByDescending(ca => ca.PlannedStartDateTime)
                .First();

            var agreedActions = DataContext.AgreedClientActions
                .Where(ac => ac.IndividualPlan.ValidUntilDate > DateTime.Today)
                .ToList();

            var startDate = lastGeneratedAction.ActionStartedDateTime.Value;
            var endDate = startDate.AddMonths(1).AddDays(-1);

            while (startDate <= endDate)
            {

                startDate = startDate.AddDays(1).Date;
            }
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
                var picture = CreatePictureFromUri(dto.Photo.PictureUri);
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
