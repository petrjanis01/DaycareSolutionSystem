using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.Actions;
using DaycareSolutionSystem.Api.Host.Controllers.Clients;
using DaycareSolutionSystem.Api.Host.Services.IndividualPlans;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers.IndividualPlans
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualPlansController : ControllerBase
    {
        private readonly IIndividualPlansApiService _individualPlansApiService;

        public IndividualPlansController(IIndividualPlansApiService individualPlansService)
        {
            _individualPlansApiService = individualPlansService;
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public void DeletePlan(Guid id)
        {
            _individualPlansApiService.DeleteIndividualPlan(id);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public void CreatePlan(IndividualPlanCreateUpdateDTO dto)
        {
            var plan = MapUpdateDtoToIndividualPlan(dto);

            _individualPlansApiService.CreateIndividualPlan(plan);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public void UpdatePlan(IndividualPlanCreateUpdateDTO dto)
        {
            var plan = MapUpdateDtoToIndividualPlan(dto);

            _individualPlansApiService.UpdateIndividualPlan(plan);
        }

        [HttpGet]
        [Route("agreed-actions-by-plans")]
        public IndividualPlanDTO[] GetClientAgreedActionsByPlans(Guid clientId)
        {
            var actionsByPlans = _individualPlansApiService.GetClientAgreedActionsByPlans(clientId);

            var individualPlans = new List<IndividualPlanDTO>();
            foreach (var entry in actionsByPlans)
            {
                var dto = MapIndividualPlanWithActionsToDto(entry.Key, entry.Value);
                individualPlans.Add(dto);
            }

            return individualPlans.ToArray();
        }

        // Mappers
        private IndividualPlanDTO MapIndividualPlanWithActionsToDto(IndividualPlan plan, List<AgreedClientAction> actions)
        {
            var actionsForDays = new List<AgreedActionsForDayDTO>();

            var actionsGrouped = actions.GroupBy(a => a.Day);

            foreach (var group in actionsGrouped)
            {
                var day = group.Key;
                var actionsForDay = new AgreedActionsForDayDTO();
                actionsForDay.Day = day;

                var actionsDtos = new List<AgreedActionDTO>();
                foreach (var action in group)
                {
                    var agreedActionDto = new AgreedActionDTO();
                    agreedActionDto.Id = action.Id;
                    agreedActionDto.EstimatedDurationMinutes = action.EstimatedDurationMinutes;
                    agreedActionDto.ClientActionSpecificDescription = action.ClientActionSpecificDescription;
                    agreedActionDto.PlannedStartTime = DateTime.Today.Add(action.PlannedStartTime);
                    agreedActionDto.PlannedEndTime = DateTime.Today.Add(action.PlannedStartTime.Add(TimeSpan.FromMinutes(action.EstimatedDurationMinutes)));

                    var actionDto = new ActionDTO();
                    actionDto.Id = action.ActionId;
                    actionDto.GeneralDescription = action.Action.GeneralDescription;
                    actionDto.Name = action.Action.Name;

                    agreedActionDto.Action = actionDto;
                    actionsDtos.Add(agreedActionDto);
                }

                actionsDtos = actionsDtos.OrderBy(a => a.PlannedStartTime).ToList();
                actionsForDay.AgreedActions = actionsDtos.ToArray();
                actionsForDays.Add(actionsForDay);
            }

            var planDto = new IndividualPlanDTO();
            planDto.Id = plan.Id;
            planDto.ValidFrom = plan.ValidFromDate;
            planDto.ValidUntil = plan.ValidUntilDate;
            planDto.ActionsForDay = actionsForDays.ToArray();

            return planDto;
        }

        private IndividualPlan MapUpdateDtoToIndividualPlan(IndividualPlanCreateUpdateDTO dto)
        {
            var plan = new IndividualPlan();
            if (dto.Id.HasValue)
            {
                plan.Id = dto.Id.Value;
            }

            plan.ClientId = dto.ClientId;
            plan.ValidFromDate = dto.ValidFrom;
            plan.ValidUntilDate = dto.ValidUntil;

            return plan;
        }

    }
}