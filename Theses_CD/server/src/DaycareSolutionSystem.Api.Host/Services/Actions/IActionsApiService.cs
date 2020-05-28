using System;
using System.Collections.Generic;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Api.Host.Services.Actions
{
    public interface IActionsApiService
    {
        List<Action> GetAllActions();

        Action GetSingleAction(Guid id);

        void CreateAction(Action action);

        void UpdateAction(Action action);

        bool DeleteAction(Guid id);
    }
}
