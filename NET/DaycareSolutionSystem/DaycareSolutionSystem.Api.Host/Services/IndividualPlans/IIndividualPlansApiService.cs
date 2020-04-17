using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.IndividualPlans
{
    public interface IIndividualPlansApiService
    {
        Dictionary<IndividualPlan, List<AgreedClientAction>> GetClientAgreedActionsByPlans(Guid clientId);

        void CreateIndividualPlan(IndividualPlan plan);

        void UpdateIndividualPlan(IndividualPlan plan);

        void DeleteIndividualPlan(Guid id);
    }
}
