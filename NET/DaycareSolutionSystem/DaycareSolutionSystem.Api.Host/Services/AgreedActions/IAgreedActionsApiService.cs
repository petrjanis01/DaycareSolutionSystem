using System;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.AgreedActions
{
    public interface IAgreedActionsApiService
    {
        AgreedClientAction GetSingleAgreedClientAction(Guid id);

        void DeleteAgreedClientAction(Guid id);

        void UpdateAgreedClientAction(AgreedClientAction action);

        void CreateAgreedClientAction(AgreedClientAction action);
    }
}
