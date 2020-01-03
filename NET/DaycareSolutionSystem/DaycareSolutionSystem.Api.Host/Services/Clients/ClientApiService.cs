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
