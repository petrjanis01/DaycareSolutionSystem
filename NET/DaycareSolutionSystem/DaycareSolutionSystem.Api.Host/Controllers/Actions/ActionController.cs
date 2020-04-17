using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Api.Host.Services.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Controllers.Actions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActionController : DssBaseController
    {
        private readonly IActionsApiService _actionsApiService;

        public ActionController(IActionsApiService actionsApiService)
        {
            _actionsApiService = actionsApiService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionDTO[] GetAllActions()
        {
            var actions = _actionsApiService.GetAllActions();

            var dtos = new List<ActionDTO>();

            foreach (var action in actions)
            {
                var dto = MapActionToDto(action);
                dtos.Add(dto);
            }

            return dtos.ToArray();
        }

        [HttpGet]
        [Route("single-action")]
        [Authorize(Roles = "Manager")]
        public ActionDTO GetAction(Guid id)
        {
            var action = _actionsApiService.GetSingleAction(id);
            var dto = MapActionToDto(action);

            return dto;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public void CreateAction(ActionDTO dto)
        {
            var action = MapDtoToAction(dto);
            _actionsApiService.CreateAction(action);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public void UpdateAction(ActionDTO dto)
        {
            var action = MapDtoToAction(dto);
            _actionsApiService.UpdateAction(action);
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public void DeleteAction(Guid id)
        {
            _actionsApiService.DeleteAction(id);
        }

        private ActionDTO MapActionToDto(Action action)
        {
            var dto = new ActionDTO();
            dto.Id = action.Id;
            dto.Name = action.Name;
            dto.GeneralDescription = action.GeneralDescription;

            return dto;
        }

        private Action MapDtoToAction(ActionDTO dto)
        {
            var action = new Action();
            if (dto.Id.HasValue)
            {
                action.Id = dto.Id.Value;
            }

            action.Name = dto.Name;
            action.GeneralDescription = dto.GeneralDescription;

            return action;
        }

    }
}