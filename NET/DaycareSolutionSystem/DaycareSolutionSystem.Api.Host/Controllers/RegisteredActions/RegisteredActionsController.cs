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
using Microsoft.EntityFrameworkCore.Internal;
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

        [HttpPut]
        [Route("registered-action")]
        public RegisteredActionDTO UpdateRegisteredAction(RegisteredActionDTO dto)
        {
            var registeredAction = _dataContext.RegisteredClientActions.Find(dto.Id);

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
                    _dataContext.Pictures.Add(picture);
                }
            }

            registeredAction.IsCanceled = dto.IsCanceled;

            _dataContext.SaveChanges();

            return dto;
        }

        [HttpGet]
        [Route("registered-actions")]
        public RegisteredActionsForDayDTO[] GetRegisteredActions(int count, Guid? lastActionDisplayedId = null)
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