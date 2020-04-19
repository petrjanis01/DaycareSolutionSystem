using System;
using DaycareSolutionSystem.Api.Host.Controllers.Actions;
using DaycareSolutionSystem.Api.Host.Services.AgreedActions;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Controllers.AgreedActions
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreedActionController : DssBaseController
    {
        private readonly IAgreedActionsApiService _agreedActionsApiService;

        public AgreedActionController(IAgreedActionsApiService agreedActionsApiService)
        {
            _agreedActionsApiService = agreedActionsApiService;
        }

        [HttpGet]
        [Route("single-action")]
        [Authorize(Roles = "Manager")]
        public AgreedActionDto GetSingleAgreedAction(Guid id)
        {
            var action = _agreedActionsApiService.GetSingleAgreedClientAction(id);
            var dto = MapActionToDto(action);

            return dto;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public void CreateAgreedAction(AgreedActionDto dto)
        {
            var action = MapDtoToAction(dto);
            _agreedActionsApiService.CreateAgreedClientAction(action);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public void UpdateAgreedAction(AgreedActionDto dto)
        {
            var action = MapDtoToAction(dto);
            _agreedActionsApiService.UpdateAgreedClientAction(action);
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public void DeleteAgreedAction(Guid id)
        {
            _agreedActionsApiService.DeleteAgreedClientAction(id);
        }

        // mappers
        private AgreedActionDto MapActionToDto(AgreedClientAction action)
        {
            var dto = new AgreedActionDto();
            dto.Id = action.Id;
            dto.ClientActionSpecificDescription = action.ClientActionSpecificDescription;
            dto.EstimatedDurationMinutes = action.EstimatedDurationMinutes;
            dto.PlannedStartTime = DateTime.Today.Add(action.PlannedStartTime);
            dto.PlannedEndTime = DateTime.Today.Add(action.PlannedStartTime).AddMinutes(action.EstimatedDurationMinutes);
            dto.Action = MapActionToDto(action.Action);
            dto.IndividualPlanId = action.IndividualPlanId;
            dto.EmployeeId = action.EmployeeId;
            dto.Day = action.Day;

            return dto;
        }

        private AgreedClientAction MapDtoToAction(AgreedActionDto dto)
        {
            var action = new AgreedClientAction();
            if (dto.Id.HasValue)
            {
                action.Id = dto.Id.Value;
            }

            action.ActionId = dto.Action.Id.Value;
            action.PlannedStartTime = dto.PlannedStartTime.TimeOfDay;

            if (dto.EstimatedDurationMinutes.HasValue)
            {
                action.EstimatedDurationMinutes = dto.EstimatedDurationMinutes.Value;
            }
            else
            {
                var duration = (int)(dto.PlannedEndTime - dto.PlannedStartTime).TotalMinutes;
                action.EstimatedDurationMinutes = duration;
            }

            action.ClientActionSpecificDescription = dto.ClientActionSpecificDescription;
            action.Day = dto.Day;
            action.EmployeeId = dto.EmployeeId;
            action.IndividualPlanId = dto.IndividualPlanId;

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