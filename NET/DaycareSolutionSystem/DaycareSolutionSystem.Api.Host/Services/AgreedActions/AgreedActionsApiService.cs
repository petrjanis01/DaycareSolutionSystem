using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.AgreedActions
{
    public class AgreedActionsApiService : ApiServiceBase, IAgreedActionsApiService
    {

        public AgreedActionsApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        { }


        public AgreedClientAction GetSingleAgreedClientAction(Guid id)
        {
            var action = DataContext.AgreedClientActions.Find(id);

            return action;
        }

        public bool DeleteAgreedClientAction(Guid id)
        {
            var action = DataContext.AgreedClientActions.Find(id);

            if (action.RegisteredClientActions.Any() == false)
            {
                DataContext.AgreedClientActions.Remove(action);
                DataContext.SaveChanges();

                return true;
            }

            return false;
        }

        public void UpdateAgreedClientAction(AgreedClientAction action)
        {
            var queriedAction = DataContext.AgreedClientActions.Find(action.Id);
            queriedAction.PlannedStartTime = action.PlannedStartTime;
            queriedAction.EstimatedDurationMinutes = action.EstimatedDurationMinutes;
            queriedAction.ActionId = action.ActionId;
            queriedAction.EmployeeId = action.EmployeeId;
            queriedAction.ClientActionSpecificDescription = action.ClientActionSpecificDescription;

            DataContext.SaveChanges();
        }

        public void CreateAgreedClientAction(AgreedClientAction action)
        {
            DataContext.AgreedClientActions.Add(action);
            DataContext.SaveChanges();
        }
    }
}
