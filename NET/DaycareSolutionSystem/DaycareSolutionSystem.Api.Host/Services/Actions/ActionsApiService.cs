using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.AspNetCore.Http;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Services.Actions
{
    public class ActionsApiService : ApiServiceBase, IActionsApiService
    {

        public ActionsApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
        }

        public List<Action> GetAllActions()
        {
            var actions = DataContext.Actions.ToList();

            return actions;
        }

        public Action GetSingleAction(Guid id)
        {
            var action = DataContext.Actions.Find(id);

            return action;
        }

        public void CreateAction(Action action)
        {
            DataContext.Actions.Add(action);
            DataContext.SaveChanges();
        }

        public void UpdateAction(Action action)
        {
            var queriedAction = DataContext.Actions.Find(action);
            queriedAction.Name = action.Name;
            queriedAction.GeneralDescription = action.GeneralDescription;
            DataContext.SaveChanges();
        }

        public void DeleteAction(Guid id)
        {
            var action = DataContext.Actions.Find(id);
            DataContext.Actions.Remove(action);
            DataContext.SaveChanges();
        }
    }
}
