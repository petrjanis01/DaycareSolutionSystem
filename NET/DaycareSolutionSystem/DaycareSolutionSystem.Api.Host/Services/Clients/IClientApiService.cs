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
    }
}
