using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Database.Migrator;
using DaycareSolutionSystem.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Action = DaycareSolutionSystem.Database.Entities.Entities.Action;

namespace DaycareSolutionSystem.Helpers
{
    public class DemoDataInitializer
    {
        private readonly DssDataContext _dataContext;
        private readonly ImageFetcher _imageFetcher;

        private static readonly string[] MaleNames = new[] { "Liam", "Noah", "William", "James", "Oliver", "Benjamin", "Lucas" };
        private static readonly string[] FemaleNames = new[] { "Emma", "Olivia", "Ava", "Isabella", "Sophia", "Charlotte", "Mia" };
        private static readonly string[] LastNames = new[] { "Smith", " Johnson", "Williams", "Jones", "Brown", "Davis", "Miller" };

        private Guid _dcEmployeeId;
        private Guid _mngrEmployeeId;
        private readonly List<Guid> _clientIds = new List<Guid>();
        private readonly List<Guid> _actionIds = new List<Guid>();

        public DemoDataInitializer(DssDataContext dataContext)
        {
            _dataContext = dataContext;
            _imageFetcher = new ImageFetcher();
        }

        public void DatabaseInit()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.Migrate();
        }

        public void CreateDemoData()
        {
            CreateEmployees();
            CreateUserAccounts();
            CreateClientsWithAddress();
            CreateActions();
            CreateIndividualPlansWithAgreedClientActions();
            CreateRegisteredClientActionsFromAgreedClientActions();
        }

        private void CreateEmployees()
        {
            var dcEmployee = new Employee();
            dcEmployee.Gender = Gender.Male;
            dcEmployee.FirstName = "Daycare";
            dcEmployee.Surname = "Employee";
            dcEmployee.Email = "dcemp@dss.com";
            dcEmployee.PhoneNumber = "+420123456789";
            dcEmployee.Birthdate = new DateTime(1990, 6, 2);
            dcEmployee.EmployeePosition = EmployeePosition.Caregiver;

            var mngrEmployee = new Employee();
            mngrEmployee.Gender = Gender.Female;
            mngrEmployee.FirstName = "Manager";
            mngrEmployee.Surname = "Employee";
            dcEmployee.Email = "mngr@dss.com";
            dcEmployee.PhoneNumber = "+420123456789";
            mngrEmployee.Birthdate = new DateTime(1995, 1, 1);
            mngrEmployee.EmployeePosition = EmployeePosition.Manager;

            _dcEmployeeId = dcEmployee.Id;
            _mngrEmployeeId = mngrEmployee.Id;

            _dataContext.Employees.AddRange(new[]
            {
                dcEmployee,
                mngrEmployee
            });

            _dataContext.SaveChanges();
        }

        private void CreateUserAccounts()
        {
            var user = new User();
            user.EmployeeId = _dcEmployeeId;
            user.LoginName = "dcemp";
            user.Password = PasswordHasher.HashPassword("1234");

            var mngr = new User();
            mngr.EmployeeId = _mngrEmployeeId;
            mngr.LoginName = "mngr";
            mngr.Password = PasswordHasher.HashPassword("1234");


            _dataContext.Users.AddRange(new[]
            {
                user,
                mngr
            });

            _dataContext.SaveChanges();
        }

        private void CreateClientsWithAddress()
        {
            var clients = new List<Client>();

            for (var i = 0; i < 10; i++)
            {
                var client = new Client();
                client.Gender = i < 5 ? Gender.Male : Gender.Female;
                client.FirstName = i < 5 ? MaleNames[i] : FemaleNames[i - 4];
                client.Surname = i < 6 ? LastNames[i] : LastNames[i - 6];
                client.Birthdate = new DateTime(1945 + i, 1 + i, 1 + i);
                var emailName = client.FullName.Replace(" ", "");
                client.Email = $"{emailName}@fakeDomain.com";
                client.PhoneNumber = $"+420{new string('1', 9)}";

                clients.Add(client);
                _clientIds.Add(client.Id);
            }

            var addresses = new List<Address>();

            var address = new Address();
            address.City = "Olomouc";
            address.PostCode = "77900";
            address.Street = "Foerstrova";
            address.BuildingNumber = "1133/59";
            addresses.Add(address);
            clients[0].AddressId = address.Id;

            var address1 = new Address();
            address1.City = "Olomouc";
            address1.PostCode = "77900";
            address1.Street = "Jílová";
            address1.BuildingNumber = "532/8";
            addresses.Add(address1);
            clients[1].AddressId = address1.Id;

            var address2 = new Address();
            address2.City = "Olomouc";
            address2.PostCode = "77900";
            address2.Street = "I.P. Pavlova";
            address2.BuildingNumber = "853/14";
            addresses.Add(address2);
            clients[2].AddressId = address2.Id;

            var address3 = new Address();
            address3.City = "Olomouc";
            address3.PostCode = "77900";
            address3.Street = "V Křovinách";
            address3.BuildingNumber = "443";
            addresses.Add(address3);
            clients[3].AddressId = address3.Id;

            var address4 = new Address();
            address4.City = "Olomouc";
            address4.PostCode = "77900";
            address4.Street = "Želivského";
            address4.BuildingNumber = "509";
            addresses.Add(address4);
            clients[4].AddressId = address4.Id;

            var address5 = new Address();
            address5.City = "Olomouc";
            address5.PostCode = "77900";
            address5.Street = "Resslova";
            address5.BuildingNumber = "225";
            addresses.Add(address5);
            clients[5].AddressId = address5.Id;

            var address6 = new Address();
            address6.Coordinates = new Coordinates();
            address6.Coordinates.Latitude = "49.594566";
            address6.Coordinates.Longitude = "17.248407";
            addresses.Add(address6);
            clients[6].AddressId = address6.Id;

            var address7 = new Address();
            address7.Coordinates = new Coordinates();
            address7.Coordinates.Latitude = "49.599403";
            address7.Coordinates.Longitude = "17.254586";
            addresses.Add(address7);
            clients[7].AddressId = address7.Id;

            var address8 = new Address();
            address8.Coordinates = new Coordinates();
            address8.Coordinates.Latitude = "49.595875";
            address8.Coordinates.Longitude = "17.249870";
            addresses.Add(address8);
            clients[8].AddressId = address8.Id;

            var address9 = new Address();
            address9.Coordinates = new Coordinates();
            address9.Coordinates.Latitude = "49.592909";
            address9.Coordinates.Longitude = "17.229890";
            addresses.Add(address9);
            clients[9].AddressId = address9.Id;

            // Profile pictures
            AssignProfilePictureToClientSafe(clients[0], DemoDataImagesUris.MaleImgUris[0]);
            AssignProfilePictureToClientSafe(clients[1], DemoDataImagesUris.MaleImgUris[1]);
            AssignProfilePictureToClientSafe(clients[2], DemoDataImagesUris.MaleImgUris[2]);
            AssignProfilePictureToClientSafe(clients[3], DemoDataImagesUris.MaleImgUris[3]);
            AssignProfilePictureToClientSafe(clients[5], DemoDataImagesUris.FemaleImgUris[0]);
            AssignProfilePictureToClientSafe(clients[6], DemoDataImagesUris.FemaleImgUris[1]);
            AssignProfilePictureToClientSafe(clients[7], DemoDataImagesUris.FemaleImgUris[2]);
            AssignProfilePictureToClientSafe(clients[8], DemoDataImagesUris.FemaleImgUris[3]);

            _dataContext.Addresses.AddRange(addresses);
            _dataContext.Clients.AddRange(clients);
            _dataContext.SaveChanges();
        }

        private void AssignProfilePictureToClientSafe(Client client, string pictureUri)
        {
            var picture = _imageFetcher.DownloadImageFromUrl(pictureUri);

            if (picture != null)
            {
                client.ProfilePicture = picture;
            }
        }

        private void CreateActions()
        {
            var actions = new List<Action>();

            var action = new Action();
            action.Name = "Bathing";
            action.GeneralDescription = "Wear gloves when you help with bathing.";
            actions.Add(action);

            var action1 = new Action();
            action1.Name = "Cleaning";
            action1.GeneralDescription = "Watch out for the dust.";
            actions.Add(action1);

            var action2 = new Action();
            action2.Name = "Grocery shopping";
            action2.GeneralDescription = "Take list with items from client first.";
            actions.Add(action2);

            var action3 = new Action();
            action3.Name = "Drugs shopping";
            action3.GeneralDescription = "Check prescriptions first.";
            actions.Add(action3);

            var action4 = new Action();
            action4.Name = "Transportation";
            action4.GeneralDescription = "No description.";
            actions.Add(action4);

            var action5 = new Action();
            action5.Name = "Errands";
            action5.GeneralDescription = "Discuss what exactly is needed with client.";
            actions.Add(action5);

            actions.ForEach(a => _actionIds.Add(a.Id));

            _dataContext.AddRange(actions);
            _dataContext.SaveChanges();
        }

        private void CreateIndividualPlansWithAgreedClientActions()
        {
            var individualPlans = new List<IndividualPlan>();
            var agreedClientActions = new List<AgreedClientAction>();
            var firstDayOfCurrentYear = new DateTime(DateTime.Today.Year, 1, 1);
            var lastDayOfCurrentYear = new DateTime(DateTime.Today.Year, 12, 31);

            var individualPlanClient = new IndividualPlan();
            individualPlanClient.ClientId = _clientIds[0];
            individualPlanClient.ValidFromDate = firstDayOfCurrentYear;
            individualPlanClient.ValidUntilDate = lastDayOfCurrentYear;
            individualPlans.Add(individualPlanClient);

            var client0Bathing = new AgreedClientAction();
            client0Bathing.ActionId = _actionIds[0];
            client0Bathing.EstimatedDurationMinutes = 60;
            client0Bathing.IndividualPlanId = individualPlanClient.Id;
            client0Bathing.Day = DayOfWeek.Monday;
            client0Bathing.PlannedStartTime = TimeSpan.FromHours(8);
            agreedClientActions.Add(client0Bathing);

            var client0Bathing1 = new AgreedClientAction();
            client0Bathing1.ActionId = _actionIds[1];
            client0Bathing1.EstimatedDurationMinutes = 60;
            client0Bathing1.IndividualPlanId = individualPlanClient.Id;
            client0Bathing1.Day = DayOfWeek.Thursday;
            client0Bathing1.PlannedStartTime = TimeSpan.FromHours(8);
            agreedClientActions.Add(client0Bathing1);

            var client0Cleaning = new AgreedClientAction();
            client0Cleaning.ActionId = _actionIds[1];
            client0Cleaning.EstimatedDurationMinutes = 30;
            client0Cleaning.IndividualPlanId = individualPlanClient.Id;
            client0Cleaning.Day = DayOfWeek.Monday;
            client0Cleaning.PlannedStartTime = TimeSpan.FromHours(9);
            agreedClientActions.Add(client0Cleaning);

            var client0Cleaning1 = new AgreedClientAction();
            client0Cleaning1.ActionId = _actionIds[1];
            client0Cleaning1.EstimatedDurationMinutes = 30;
            client0Cleaning1.IndividualPlanId = individualPlanClient.Id;
            client0Cleaning1.Day = DayOfWeek.Thursday;
            client0Cleaning1.PlannedStartTime = TimeSpan.FromHours(9);
            agreedClientActions.Add(client0Cleaning1);

            var client0GroceryShopping = new AgreedClientAction();
            client0GroceryShopping.ActionId = _actionIds[2];
            client0GroceryShopping.EstimatedDurationMinutes = 60;
            client0GroceryShopping.IndividualPlanId = individualPlanClient.Id;
            client0GroceryShopping.Day = DayOfWeek.Wednesday;
            client0GroceryShopping.PlannedStartTime = TimeSpan.FromHours(8);
            agreedClientActions.Add(client0GroceryShopping);

            var individualPlanClient1 = new IndividualPlan();
            individualPlanClient1.ClientId = _clientIds[1];
            individualPlanClient1.ValidFromDate = firstDayOfCurrentYear;
            individualPlanClient1.ValidUntilDate = lastDayOfCurrentYear;
            individualPlans.Add(individualPlanClient1);

            var client1GroceryShopping = new AgreedClientAction();
            client1GroceryShopping.ActionId = _actionIds[2];
            client1GroceryShopping.EstimatedDurationMinutes = 60;
            client1GroceryShopping.IndividualPlanId = individualPlanClient1.Id;
            client1GroceryShopping.Day = DayOfWeek.Tuesday;
            client1GroceryShopping.PlannedStartTime = TimeSpan.FromHours(8);
            agreedClientActions.Add(client1GroceryShopping);

            var client1Transportation = new AgreedClientAction();
            client1Transportation.ActionId = _actionIds[4];
            client1Transportation.EstimatedDurationMinutes = 120;
            client1Transportation.IndividualPlanId = individualPlanClient1.Id;
            client1Transportation.Day = DayOfWeek.Tuesday;
            client1Transportation.PlannedStartTime = TimeSpan.FromHours(9);
            agreedClientActions.Add(client1Transportation);

            var client1Errands = new AgreedClientAction();
            client1Errands.ActionId = _actionIds[5];
            client1Errands.EstimatedDurationMinutes = 60;
            client1Errands.IndividualPlanId = individualPlanClient1.Id;
            client1Errands.Day = DayOfWeek.Tuesday;
            client1Errands.PlannedStartTime = TimeSpan.FromHours(1);
            agreedClientActions.Add(client1Errands);

            var individualPlanClient2 = new IndividualPlan();
            individualPlanClient2.ClientId = _clientIds[2];
            individualPlanClient2.ValidFromDate = firstDayOfCurrentYear;
            individualPlanClient2.ValidUntilDate = lastDayOfCurrentYear;
            individualPlans.Add(individualPlanClient2);

            var client2Cleaning = new AgreedClientAction();
            client2Cleaning.ActionId = _actionIds[1];
            client2Cleaning.EstimatedDurationMinutes = 30;
            client2Cleaning.IndividualPlanId = individualPlanClient2.Id;
            client2Cleaning.Day = DayOfWeek.Monday;
            client2Cleaning.PlannedStartTime = TimeSpan.FromHours(10);
            agreedClientActions.Add(client2Cleaning);

            var client2Cleaning2 = new AgreedClientAction();
            client2Cleaning2.ActionId = _actionIds[1];
            client2Cleaning2.EstimatedDurationMinutes = 30;
            client2Cleaning2.IndividualPlanId = individualPlanClient2.Id;
            client2Cleaning2.Day = DayOfWeek.Wednesday;
            client2Cleaning2.PlannedStartTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30));
            agreedClientActions.Add(client2Cleaning2);

            var individualPlanClient3 = new IndividualPlan();
            individualPlanClient3.ClientId = _clientIds[3];
            individualPlanClient3.ValidFromDate = firstDayOfCurrentYear;
            individualPlanClient3.ValidUntilDate = lastDayOfCurrentYear;
            individualPlans.Add(individualPlanClient3);

            var client3Errands = new AgreedClientAction();
            client3Errands.ActionId = _actionIds[3];
            client3Errands.EstimatedDurationMinutes = 120;
            client3Errands.IndividualPlanId = individualPlanClient3.Id;
            client3Errands.Day = DayOfWeek.Friday;
            client3Errands.PlannedStartTime = TimeSpan.FromHours(8);
            agreedClientActions.Add(client2Cleaning);

            var client3DrugShopping = new AgreedClientAction();
            client3DrugShopping.ActionId = _actionIds[3];
            client3DrugShopping.EstimatedDurationMinutes = 60;
            client3DrugShopping.IndividualPlanId = individualPlanClient3.Id;
            client3DrugShopping.Day = DayOfWeek.Friday;
            client3DrugShopping.PlannedStartTime = TimeSpan.FromHours(10);
            agreedClientActions.Add(client3DrugShopping);

            var individualPlanClient5 = new IndividualPlan();
            individualPlanClient5.ClientId = _clientIds[5];
            individualPlanClient5.ValidFromDate = firstDayOfCurrentYear;
            individualPlanClient5.ValidUntilDate = lastDayOfCurrentYear;
            individualPlans.Add(individualPlanClient5);

            var client5Bathing = new AgreedClientAction();
            client5Bathing.ActionId = _actionIds[0];
            client5Bathing.EstimatedDurationMinutes = 60;
            client5Bathing.IndividualPlanId = individualPlanClient5.Id;
            client5Bathing.Day = DayOfWeek.Monday;
            client5Bathing.PlannedStartTime = TimeSpan.FromHours(11);
            agreedClientActions.Add(client5Bathing);

            foreach (var agreedClientAction in agreedClientActions)
            {
                agreedClientAction.EmployeeId = _dcEmployeeId;
            }

            _dataContext.IndividualPlans.AddRange(individualPlans);
            _dataContext.AgreedClientActions.AddRange(agreedClientActions);
            _dataContext.SaveChanges();
        }

        private void CreateRegisteredClientActionsFromAgreedClientActions()
        {
            var agreedClientActions = _dataContext.AgreedClientActions.ToList();
            var registeredClientActions = new List<RegisteredClientAction>();

            foreach (var agreedClientAction in agreedClientActions)
            {
                agreedClientAction.ClientActionSpecificDescription = $"This is some long and detailed description how to do {agreedClientAction.Action.Name} " +
                                                                     $"for client {agreedClientAction.IndividualPlan.Client.FullName} on {agreedClientAction.Day}.";
                registeredClientActions.AddRange(GenerateRegisteredClientActionsForAgreedClientAction(agreedClientAction));
            }

            _dataContext.RegisteredClientActions.AddRange(registeredClientActions);
            _dataContext.SaveChanges();
        }

        // create registered client actions from agreed client actions - all past and max 30 days into future
        private List<RegisteredClientAction> GenerateRegisteredClientActionsForAgreedClientAction(
            AgreedClientAction agreedClientAction)
        {
            var registeredClientActions = new List<RegisteredClientAction>();
            var startDate = agreedClientAction.IndividualPlan.ValidFromDate;

            if (startDate > DateTime.Today.AddDays(10))
            {
                return registeredClientActions;
            }

            var endDate = agreedClientAction.IndividualPlan.ValidUntilDate > DateTime.Today.AddDays(30)
                ? DateTime.Today.AddDays(10)
                : agreedClientAction.IndividualPlan.ValidUntilDate;

            var random = new Random();

            while (startDate < endDate)
            {
                if (startDate.DayOfWeek != agreedClientAction.Day)
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                var clientAction = new RegisteredClientAction();
                clientAction.EmployeeId = agreedClientAction.EmployeeId;
                clientAction.ClientId = agreedClientAction.IndividualPlan.ClientId;
                clientAction.AgreedClientAction = agreedClientAction;
                clientAction.PlannedStartDateTime = startDate.Add(agreedClientAction.PlannedStartTime);
                clientAction.IsCanceled = random.Next(1, 10) == 1;
                clientAction.IsCompleted = clientAction.IsCanceled == false && startDate < DateTime.Today;
                clientAction.ActionId = agreedClientAction.ActionId;

                if (clientAction.IsCanceled)
                {
                    clientAction.Comment = "Reason for canceling action.";
                }

                if (clientAction.IsCompleted)
                {
                    clientAction.ActionStartedDateTime = startDate.Add(agreedClientAction.PlannedStartTime);
                    clientAction.ActionFinishedDateTime = clientAction.ActionStartedDateTime
                        ?.AddMinutes(agreedClientAction.EstimatedDurationMinutes);
                }

                registeredClientActions.Add(clientAction);

                startDate = startDate.AddDays(1);
            }

            return registeredClientActions;
        }
    }
}
