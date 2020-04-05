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

        // TODO unit test this
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

            client.ProfilePicture.MimeType = clientUpdated.ProfilePicture.MimeType;
            client.ProfilePicture.BinaryData = clientUpdated.ProfilePicture.BinaryData;

            client.Address.City = clientUpdated.Address.City;
            client.Address.BuildingNumber = clientUpdated.Address.BuildingNumber;
            client.Address.PostCode = clientUpdated.Address.PostCode;
            client.Address.Street = clientUpdated.Address.Street;

            client.Address.Coordinates.Latitude = clientUpdated.Address.Coordinates.Latitude;
            client.Address.Coordinates.Longitude = clientUpdated.Address.Coordinates.Longitude;

            DataContext.SaveChanges();

            return client;
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

        // TODO check this method (probably not returning correct data)
        public List<RegisteredClientAction> GetNextRegisteredActionsToday(Guid? employeeId = null)
        {
            employeeId ??= GetCurrentUser()?.Employee.Id;

            var clients = DataContext.RegisteredClientActions
                .Where(ca => ca.PlannedStartDateTime.Date == DateTime.Today)
                .Where(ca => ca.EmployeeId == employeeId)
                .Where(ca => ca.IsCanceled == false)
                .Select(ca => ca.Client).ToList();

            var clientActions = new List<RegisteredClientAction>();
            foreach (var client in clients)
            {
                var futureClientActions =
                    DataContext.RegisteredClientActions.Where(ca => ca.ClientId == client.Id)
                        .Where(ca => ca.PlannedStartDateTime > DateTime.Now);

                var nextClientAction = futureClientActions.OrderBy(ca => ca.PlannedStartDateTime).First();
                clientActions.Add(nextClientAction);
            }

            return clientActions;
        }

        // TODO unit test for this
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
