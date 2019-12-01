using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Api.Host.Controllers.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaycareSolutionSystem.Api.Host.Services.RegisteredActions;
using DaycareSolutionSystem.Database.Entities.Entities;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Controllers.RegisteredActions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisteredActionsController : ControllerBase
    {
        private readonly IRegisteredActionsApiService _registeredActionsApiService;

        public RegisteredActionsController(IRegisteredActionsApiService registeredActionsApiService)
        {
            _registeredActionsApiService = registeredActionsApiService;
        }

        [HttpGet]
        [Route("get-registered-actions-details")]
        public RegisteredActionsForDayDTO[] GetRegisteredActionsDetails(int count, Guid? lastActionDisplayedId = null)
        {
            var actionsPerDay = _registeredActionsApiService.GetRegisteredActionsPerDay(count, lastActionDisplayedId);

            var registeredActionsForDays = new List<RegisteredActionsForDayDTO>();

            foreach (var entry in actionsPerDay)
            {
                var actions = entry.Value;

                var dto = new RegisteredActionsForDayDTO();
                dto.Date = entry.Key;
                dto.Day = dto.Date.DayOfWeek;
                dto.ContainsLast = actions[^1].IsLast;

                var registeredActions = new List<RegisteredActionDetailDTO>();

                foreach (var action in actions)
                {
                    var actionDetail = MapRegisteredActionToDto(action.RegisteredClientAction);

                    actionDetail.ActionInfo = MapActionToDto(action.Action);

                    actionDetail.ClientBasicInfo = MapClientBasicsToDto(action.Client);

                    registeredActions.Add(actionDetail);
                }

                dto.RegisteredActionDetails = registeredActions.ToArray();
                registeredActionsForDays.Add(dto);
            }

            return registeredActionsForDays.ToArray();
        }

        private RegisteredActionDetailDTO MapRegisteredActionToDto(RegisteredClientAction registeredClientAction)
        {
            var dto = new RegisteredActionDetailDTO();
            dto.Id = registeredClientAction.Id;
            dto.ActionFinishedDateTime = registeredClientAction.ActionFinishedDateTime;
            dto.ActionStartedDateTime = registeredClientAction.ActionStartedDateTime;
            dto.ClientActionSpecificDescription = registeredClientAction.Comment;
            dto.IsCanceled = registeredClientAction.IsCanceled;
            dto.IsCompleted = registeredClientAction.IsCompleted;

            // TODO photo to base64
            dto.PhotoUri = "";

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

        private ClientBasicInfoDTO MapClientBasicsToDto(Client client)
        {
            var dto = new ClientBasicInfoDTO();
            dto.Id = client.Id;
            dto.ClientFullName = client.FullName;

            // TODO photo to base64
            dto.ProfilePictureUri = "";

            return dto;
        }
    }
}