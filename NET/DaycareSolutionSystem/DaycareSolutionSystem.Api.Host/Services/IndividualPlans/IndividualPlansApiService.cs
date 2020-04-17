using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.IndividualPlans
{
    public class IndividualPlansApiService : ApiServiceBase, IIndividualPlansApiService
    {
        public IndividualPlansApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        { }

        public Dictionary<IndividualPlan, List<AgreedClientAction>> GetClientAgreedActionsByPlans(Guid clientId)
        {
            var individualPlans = DataContext.IndividualPlans.Where(ip => ip.ClientId == clientId)
                .OrderBy(ip => ip.ValidFromDate).ToList();

            var agreedActions = DataContext.AgreedClientActions
                .Where(ac => individualPlans.Select(ip => ip.Id).Contains(ac.IndividualPlanId))
                .Distinct().ToList();

            var actionsByPlans = new Dictionary<IndividualPlan, List<AgreedClientAction>>();

            foreach (var individualPlan in individualPlans)
            {
                var actionsForPlan = agreedActions
                    .Where(ac => ac.IndividualPlanId == individualPlan.Id)
                    .OrderBy(ac => ac.Day);

                actionsByPlans.Add(individualPlan, actionsForPlan.ToList());
            }

            return actionsByPlans;
        }

        public void CreateIndividualPlan(IndividualPlan plan)
        {
            DataContext.IndividualPlans.Add(plan);
            DataContext.SaveChanges();
        }

        public void UpdateIndividualPlan(IndividualPlan plan)
        {
            var queriedPlan = DataContext.IndividualPlans.Find(plan.Id);
            queriedPlan.ValidUntilDate = plan.ValidUntilDate;
            queriedPlan.ValidFromDate = plan.ValidFromDate;
            queriedPlan.ClientId = plan.ClientId;
            DataContext.SaveChanges();
        }

        public void DeleteIndividualPlan(Guid id)
        {
            var plan = DataContext.IndividualPlans.Find(id);

            if (plan.AgreedClientActions.Count == 0)
            {
                DataContext.IndividualPlans.Remove(plan);
                DataContext.SaveChanges();
            }
        }
    }
}
