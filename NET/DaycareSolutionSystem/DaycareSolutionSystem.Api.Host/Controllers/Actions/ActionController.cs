using System;
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

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionDTO[] GetAllActions()
        {
            return new ActionDTO[1];
        }

        [HttpGet]
        [Route("single-action")]
        [Authorize(Roles = "Manager")]
        public ActionDTO GetAction(Guid id)
        {
            return new ActionDTO();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public void CreateAction(ActionDTO dto)
        {

        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public void UpdateAction(ActionDTO dto)
        {

        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public void DeleteAction(Guid id)
        {

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