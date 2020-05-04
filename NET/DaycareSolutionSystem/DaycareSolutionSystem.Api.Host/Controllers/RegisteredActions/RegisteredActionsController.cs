using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.Actions;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaycareSolutionSystem.Api.Host.Services.RegisteredActions;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Helpers;
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

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("all-actions-month")]
        public RegisteredActionDTO[] GetAllRegisteredClientActionsInGivenMonth(DateTime date, Guid? clientId = null,
            Guid? employeeId = null)
        {
            var registeredActions =
                _registeredActionsApiService.GetAllRegisteredClientActionsInGivenMonth(date, clientId, employeeId);

            var dtos = new List<RegisteredActionDTO>();

            foreach (var registeredClientAction in registeredActions)
            {
                var dto = MapRegisteredActionToDto(registeredClientAction);
                dtos.Add(dto);
            }

            return dtos.ToArray();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("generate-actions-for-period")]
        public void GenerateRegisteredActionsForPeriod(DateTime? fromDate = null, DateTime? untilDate = null)
        {
            _registeredActionsApiService.GenerateRegisteredActionsForPeriod(fromDate, untilDate);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public void CreateRegisteredAction(RegisteredActionDTO dto)
        {
            var action = MapDtoToRegisteredAction(dto);
            _registeredActionsApiService.CreateRegisteredAction(action);
        }

        [HttpPut]
        [Route("registered-action")]
        public RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto)
        {
            var action = MapDtoToRegisteredAction(dto);
            var updatedAction = _registeredActionsApiService.UpdateRegisteredAction(action);
            var updatedDto = MapRegisteredActionToDto(updatedAction);

            return updatedDto;
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

        // mappers
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
            dto.EstimatedDurationMinutes = registeredClientAction.AgreedClientActionId.HasValue
                    ? registeredClientAction.AgreedClientAction.EstimatedDurationMinutes
                    : registeredClientAction.EstimatedDurationMinutes.Value;
            dto.ClientActionSpecificDescription = registeredClientAction.AgreedClientActionId.HasValue
                ? registeredClientAction.AgreedClientAction.ClientActionSpecificDescription
                : string.Empty;
            dto.Day = registeredClientAction.AgreedClientActionId.HasValue
                ? registeredClientAction.AgreedClientAction.Day
                : registeredClientAction.PlannedStartDateTime.DayOfWeek;
            dto.Photo = new PictureDTO { PictureUri = FormatPictureToBase64(registeredClientAction.Photo) };
            dto.Action = MapActionToDto(registeredClientAction.Action);
            dto.ClientId = registeredClientAction.ClientId;
            dto.EmployeeId = registeredClientAction.EmployeeId;
            dto.ActionId = registeredClientAction.ActionId;

            return dto;
        }

        private RegisteredClientAction MapDtoToRegisteredAction(RegisteredActionDTO dto)
        {
            var action = new RegisteredClientAction();

            if (dto.Id.HasValue)
            {
                action.Id = dto.Id.Value;
            }

            action.ActionId = dto.ActionId;
            action.PlannedStartDateTime = dto.PlannedStartDateTime;
            action.EmployeeId = dto.EmployeeId;
            action.ActionFinishedDateTime = dto.ActionFinishedDateTime;
            action.ActionStartedDateTime = dto.ActionStartedDateTime;
            action.ClientId = dto.ClientId;
            action.Comment = dto.Comment;
            action.IsCanceled = dto.IsCanceled;
            action.IsCompleted = dto.IsCompleted;
            action.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
            if (dto.Photo != null)
            {
                action.Photo = Base64ImageHelper.CreatePictureFromUri(dto.Photo.PictureUri);
            }

            return action;
        }


        private ActionDTO MapActionToDto(Action action)
        {
            var dto = new ActionDTO();
            dto.Id = action.Id;
            dto.Name = action.Name;
            dto.GeneralDescription = action.GeneralDescription;

            return dto;
        }
    }
}