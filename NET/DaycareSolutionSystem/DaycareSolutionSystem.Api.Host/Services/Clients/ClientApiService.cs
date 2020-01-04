using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services.Clients
{
    public class ClientApiService : ApiServiceBase, IClientApiService
    {
        public ClientApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor) :
            base(dataContext, httpContextAccessor)
        {
        }

        public Dictionary<IndividualPlan, List<AgreedClientAction>> GetClientAgreedActionsByPlans(Guid clientId)
        {
            var individualPlans = DataContext.IndividualPlans.Where(ip => ip.ClientId == clientId)
                .OrderBy(ip => ip.ValidFromDate).ToList();

            var agreedActions = DataContext.AgreedClientActions
                .Where(ac => individualPlans.Select(ip => ip.Id).Contains(ac.IndividualPlanId)).ToList();

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

        public List<Client> GetAgreedActionsLinkedClients(Guid? employeeId)
        {
            employeeId ??= GetCurrentUser().EmployeeId;

            var clients = DataContext.AgreedClientActions
                .Where(ca => ca.EmployeeId == employeeId)
                .Select(ca => ca.IndividualPlan)
                .Select(ip => ip.Client)
                .Distinct();

            return clients.ToList();
        }

        public Client GetClient(Guid clientId)
        {
            var client = DataContext.Clients.Find(clientId);

            return client;
        }

        public Client ChangeClientProfilePicture(Guid clientId, string pictureUri)
        {
            return ChangeProfilePicture<Client>(clientId, pictureUri);
        }
    }
}
