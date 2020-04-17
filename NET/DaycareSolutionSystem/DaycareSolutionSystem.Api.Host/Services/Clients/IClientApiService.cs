using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.Clients
{
    public interface IClientApiService
    {
        List<Client> GetAgreedActionsLinkedClients(Guid? employeeId);

        Client ChangeClientProfilePicture(Guid clientId, string pictureUri);

        Client GetClient(Guid clientId);

        Dictionary<IndividualPlan, List<AgreedClientAction>> GetClientAgreedActionsByPlans(Guid clientId);

        List<RegisteredClientAction> GetNextNotStartedRegisteredActionsToday(Guid? employeeId = null);

        List<RegisteredClientAction> GetAllNextRegisteredActions(Guid? employeeId = null);

        List<Client> GetAllClients();

        Client CreateClient(Client client);

        Client UpdateClient(Client clientUpdated);

        void DeleteClient(Guid clientId);
    }
}
