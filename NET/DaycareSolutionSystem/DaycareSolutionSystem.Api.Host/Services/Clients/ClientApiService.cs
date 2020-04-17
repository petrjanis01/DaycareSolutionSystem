using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.Clients;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Helpers;
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
                .OrderBy(cl => cl.FirstName)
                .ThenBy(cl => cl.Surname)
                .Distinct();

            return clients.ToList();
        }

        public List<Client> GetAllClients()
        {
            var clients = DataContext.Clients
                .OrderBy(cl => cl.FirstName)
                .ThenBy(cl => cl.Surname);

            return clients.ToList();
        }

        public Client CreateClient(Client client)
        {
            DataContext.Clients.Add(client);
            DataContext.SaveChanges();

            return client;
        }

        public Client UpdateClient(Client clientUpdated)
        {
            var client = DataContext.Clients.FirstOrDefault(cl => cl.Id == clientUpdated.Id);

            client.FirstName = clientUpdated.FirstName;
            client.Surname = clientUpdated.Surname;
            client.PhoneNumber = clientUpdated.PhoneNumber;
            client.Email = clientUpdated.Email;
            client.Birthdate = clientUpdated.Birthdate;
            client.Gender = clientUpdated.Gender;

            if (client.ProfilePicture == null && clientUpdated.ProfilePicture != null)
            {
                DataContext.Pictures.Add(clientUpdated.ProfilePicture);
                client.ProfilePicture = clientUpdated.ProfilePicture;
                client.ProfilePictureId = clientUpdated.ProfilePictureId;

            }
            else if (clientUpdated.ProfilePicture != null)
            {
                client.ProfilePicture.MimeType = clientUpdated.ProfilePicture.MimeType;
                client.ProfilePicture.BinaryData = clientUpdated.ProfilePicture.BinaryData;
            }

            client.Address.City = clientUpdated.Address.City;
            client.Address.BuildingNumber = clientUpdated.Address.BuildingNumber;
            client.Address.PostCode = clientUpdated.Address.PostCode;
            client.Address.Street = clientUpdated.Address.Street;

            if (client.Address.Coordinates == null && clientUpdated.Address.Coordinates != null)
            {
                DataContext.Coordinates.Add(clientUpdated.Address.Coordinates);
                client.Address.Coordinates = clientUpdated.Address.Coordinates;

            }
            else if (clientUpdated.ProfilePicture != null)
            {
                client.Address.Coordinates.Latitude = clientUpdated.Address.Coordinates.Latitude;
                client.Address.Coordinates.Longitude = clientUpdated.Address.Coordinates.Longitude;
            }

            DataContext.SaveChanges();

            return client;
        }

        public void DeleteClient(Guid clientId)
        {
            var client = DataContext.Clients.Find(clientId);
            DataContext.Clients.Remove(client);
            DataContext.SaveChanges();
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

        // Returns first action for each client that is planned for today and hasn't started yet
        public List<RegisteredClientAction> GetNextNotStartedRegisteredActionsToday(Guid? employeeId = null)
        {
            employeeId ??= GetCurrentUser()?.Employee.Id;

            // Get all clients that has registered actions planned for today
            var clients = DataContext.RegisteredClientActions
                .Where(ca => ca.PlannedStartDateTime.Date == DateTime.Today)
                .Where(ca => ca.EmployeeId == employeeId)
                .Where(ca => ca.IsCanceled == false)
                .Select(ca => ca.Client)
                .Distinct().ToList();

            var clientActions = new List<RegisteredClientAction>();
            foreach (var client in clients)
            {
                // Get all actions planned for today that hasn't been done or started yet
                var futureClientActions =
                    DataContext.RegisteredClientActions.Where(ca => ca.ClientId == client.Id)
                        .Where(ca => ca.ActionStartedDateTime.HasValue == false);

                var nextClientAction = futureClientActions.OrderBy(ca => ca.PlannedStartDateTime).First();
                clientActions.Add(nextClientAction);
            }

            return clientActions;
        }

        // Gets all employee linked clients next actions that hasn't started.
        // When next registered action for any client does not exists 
        // and individual plan is still valid creates registered action from agreed action and returns it.
        public List<RegisteredClientAction> GetAllNextRegisteredActions(Guid? employeeId = null)
        {
            employeeId ??= GetCurrentUser()?.Employee.Id;

            var clients = GetAgreedActionsLinkedClients(employeeId);

            var clientActions = new List<RegisteredClientAction>();
            foreach (var client in clients)
            {
                var futureRegisteredActions =
                    DataContext.RegisteredClientActions
                        .Where(ca => ca.ClientId == client.Id)
                        .Where(ca => ca.EmployeeId == employeeId)
                        .Where(ca => ca.IsCanceled == false)
                        .Where(ca => ca.PlannedStartDateTime > DateTime.Now);

                if (futureRegisteredActions.Any() == false)
                {
                    var nextRegisteredAction = FindNextAgreedClientAction(client, employeeId.Value);
                    if (nextRegisteredAction != null)
                    {
                        clientActions.Add(nextRegisteredAction);
                    }
                }
                else
                {
                    var nextClientAction = futureRegisteredActions.OrderBy(ca => ca.PlannedStartDateTime).First();
                    if (nextClientAction != null)
                    {
                        clientActions.Add(nextClientAction);
                    }
                }
            }

            return clientActions;
        }

        private RegisteredClientAction FindNextAgreedClientAction(Client client, Guid employeeId)
        {
            var futureAgreedActions = DataContext.AgreedClientActions
                .Where(ca => ca.EmployeeId == employeeId)
                .Where(ca => ca.IndividualPlan.ClientId == client.Id)
                .Where(ca => ca.IndividualPlan.ValidFromDate.Date <= DateTime.Today
                             && ca.IndividualPlan.ValidUntilDate.Date >= DateTime.Today)
                .ToList();

            if (futureAgreedActions.Any() == false)
            {
                return null;
            }

            var date = DateTime.Today;
            var until = date.AddDays(6);

            while (date <= until)
            {
                var actionsInDay = futureAgreedActions.Where(ca => ca.Day == date.DayOfWeek);

                if (actionsInDay.Any())
                {
                    var nextAgreedAction = actionsInDay.OrderBy(ca => ca.PlannedStartTime).First();
                    var nextRegisteredAction = CreateRegisteredActionFromAgreedAction(nextAgreedAction);

                    return nextRegisteredAction;
                }

                date = date.AddDays(1);
            }

            return null;
        }

        private RegisteredClientAction CreateRegisteredActionFromAgreedAction(AgreedClientAction agreedClientAction)
        {
            var registeredAction = new RegisteredClientAction();

            registeredAction.IsCanceled = false;
            registeredAction.ClientId = agreedClientAction.IndividualPlan.ClientId;
            registeredAction.PlannedStartDateTime = GeneralHelper.GetNextDateFromDay(agreedClientAction.Day).Date
                .Add(agreedClientAction.PlannedStartTime);
            registeredAction.EmployeeId = agreedClientAction.EmployeeId;
            registeredAction.AgreedClientActionId = agreedClientAction.Id;

            return registeredAction;
        }
    }
}
