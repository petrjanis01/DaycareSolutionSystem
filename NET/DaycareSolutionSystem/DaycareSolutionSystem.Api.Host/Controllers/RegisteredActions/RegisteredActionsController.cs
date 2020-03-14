using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaycareSolutionSystem.Api.Host.Services.RegisteredActions;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Controllers.RegisteredActions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisteredActionsController : DssBaseController
    {
        private readonly IRegisteredActionsApiService _registeredActionsApiService;
        private readonly DssDataContext _dataContext;

        public RegisteredActionsController(DssDataContext dbContext, IRegisteredActionsApiService registeredActionsApiService)
        {
            _registeredActionsApiService = registeredActionsApiService;
            _dataContext = dbContext;
        }

        [HttpPost]
        [Route("generate-next-month-actions")]
        public void GenerateNextMonthRegisteredActions()
        {
            _registeredActionsApiService.GenerateNextMonthRegisteredActions();
        }

        [HttpPut]
        [Route("registered-action")]
        public RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto)
        {
            dto = _registeredActionsApiService.UpdateRegisteredAction(dto);

            return dto;
        }

        [HttpGet]
        [Route("last-registered-actions-min-max-date")]
        public MinMaxDateDTO GetRegisteredActionsMinMaxDate()
        {
            var clientActions = _dataContext.RegisteredClientActions;
            var min = clientActions.OrderBy(ca => ca.PlannedStartDateTime).First().PlannedStartDateTime.Date;
            var max = clientActions.OrderByDescending(ca => ca.PlannedStartDateTime).First().PlannedStartDateTime.Date;

            var dto = new MinMaxDateDTO();
            dto.MinDate = min;
            dto.MaxDate = max;

            return dto;
        }

        [HttpGet]
        [Route("registered-actions")]
        public RegisteredActionsForDayDTO[] GetRegisteredActions(int count, DateTime date, Guid? lastActionDisplayedId = null)
        {
            var actionsPerDay = _registeredActionsApiService.GetRegisteredActionsPerDay(count, date, lastActionDisplayedId);

            var registeredActionsForDays = new List<RegisteredActionsForDayDTO>();

            foreach (var entry in actionsPerDay)
            {
                var actions = entry.Value;

                var dto = new RegisteredActionsForDayDTO();
                dto.Date = entry.Key;
                dto.Day = dto.Date.DayOfWeek;
                dto.ContainsLast = actions[^1].IsLast;

                var actionsClient = new List<RegisteredActionsClientDTO>();
                var actionDetails = new List<RegisteredActionDTO>();

                foreach (var action in actions)
                {
                    if (actionsClient.Any() == false || actionsClient[^1].ClientId != action.Client.Id)
                    {
                        var actionClient = new RegisteredActionsClientDTO();
                        actionClient.ClientId = action.Client.Id;

                        if (actionsClient.Any())
                        {
                            actionsClient[^1].RegisteredActions = actionDetails.ToArray();
                        }

                        actionsClient.Add(actionClient);

                        actionDetails = new List<RegisteredActionDTO>();
                    }

                    var registeredAction = MapRegisteredActionToDto(action.RegisteredClientAction);
                    registeredAction.Action = MapActionToDto(action.Action);
                    actionDetails.Add(registeredAction);
                }

                if (actionsClient.Any())
                {
                    actionsClient[^1].RegisteredActions = actionDetails.ToArray();
                }

                dto.RegisteredActionsClient = actionsClient.ToArray();
                registeredActionsForDays.Add(dto);
            }

            return registeredActionsForDays.ToArray();
        }

        private RegisteredActionDTO MapRegisteredActionToDto(RegisteredClientAction registeredClientAction)
        {
            var dto = new RegisteredActionDTO();
            dto.Id = registeredClientAction.Id;
            dto.ActionFinishedDateTime = registeredClientAction.ActionFinishedDateTime;
            dto.ActionStartedDateTime = registeredClientAction.ActionStartedDateTime;
            dto.ClientActionSpecificDescription = registeredClientAction.Comment;
            dto.IsCanceled = registeredClientAction.IsCanceled;
            dto.IsCompleted = registeredClientAction.IsCompleted;
            dto.Comment = registeredClientAction.Comment;
            dto.PlannedStartDateTime = registeredClientAction.PlannedStartDateTime;
            dto.EstimatedDurationMinutes = registeredClientAction.AgreedClientAction.EstimatedDurationMinutes;
            dto.ClientActionSpecificDescription =
                registeredClientAction.AgreedClientAction.ClientActionSpecificDescription;
            dto.Day = registeredClientAction.AgreedClientAction.Day;
            dto.Photo = new PictureDTO { PictureUri = FormatPictureToBase64(registeredClientAction.Photo) };

            return dto;
        }

        private ActionDTO MapActionToDto(Action action)
        {
            var dto = new ActionDTO();
            dto.Id = action.Id;
            dto.Name = action.Name;
            dto.Description = action.GeneralDescription;

            return dto;
        }
    }
}