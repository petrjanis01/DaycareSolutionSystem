using System;
using System.Collections.Generic;
using System.Linq;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers.Clients
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : DssBaseController
    {
        private readonly IClientApiService _clientApiService;

        public ClientsController(IClientApiService clientApiService)
        {
            _clientApiService = clientApiService;
        }

        [HttpGet]
        [Route("agreed-actions-by-plans")]
        public IndividualPlanDTO[] GetClientAgreedActionsByPlans(Guid clientId)
        {
            var actionsByPlans = _clientApiService.GetClientAgreedActionsByPlans(clientId);

            var individualPlans = new List<IndividualPlanDTO>();
            foreach (var entry in actionsByPlans)
            {
                var dto = MapIndividualPlanWithActionsToDto(entry.Key, entry.Value);
                individualPlans.Add(dto);
            }

            return individualPlans.ToArray();
        }

        [HttpPost]
        [Route("change-profile-picture")]
        public IActionResult ChangeProfilePicture(Guid clientId, PictureDTO dto)
        {
            if (dto != null)
            {
                var client = _clientApiService.ChangeClientProfilePicture(clientId, dto.PictureUri);

                if (client != null)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public ClientDTO[] GetAgreedActionsLinkedClients(Guid? employeeId = null)
        {
            var clients = _clientApiService.GetAgreedActionsLinkedClients(employeeId);

            var clientDtos = new List<ClientDTO>();
            foreach (var client in clients)
            {
                var dto = MapClientToDto(client);
                clientDtos.Add(dto);
            }

            return clientDtos.ToArray();
        }

        [HttpGet]
        [Route("all-client-basics")]
        [Authorize(Roles = "Manager")]
        public ClientBasicsDTO[] GetAllClientBasics()
        {
            var clients = _clientApiService.GetAllClients();

            var clientBasicsDtos = new List<ClientBasicsDTO>();
            foreach (var client in clients)
            {
                var dto = new ClientBasicsDTO();
                dto.Id = client.Id;
                dto.FullName = client.FullName;
                dto.ProfilePicture = new PictureDTO { PictureUri = FormatPictureToBase64(client.ProfilePicture) };
                clientBasicsDtos.Add(dto);
            }

            return clientBasicsDtos.ToArray();
        }

        [HttpGet]
        [Route("single-client")]
        public ClientDTO GetClient(Guid clientId)
        {
            var client = _clientApiService.GetClient(clientId);
            var dto = MapClientToDto(client);

            return dto;
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("single-client")]
        public ClientDTO UpdateClient(ClientDTO clientDto)
        {
            var client = MapDtoToClient(clientDto);

            var updatedClient = _clientApiService.UpdateClient(client);
            var dto = MapClientToDto(updatedClient);

            return dto;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("single-client")]
        public ClientDTO CreateClient(ClientDTO clientDto)
        {
            var client = MapDtoToClient(clientDto);

            var updatedClient = _clientApiService.CreateClient(client);
            var dto = MapClientToDto(updatedClient);

            return dto;
        }

        [HttpGet]
        [Route("today-scheduled-clients")]
        public ClientWithNextActionDTO[] GetClientsScheduledToday(Guid? employeeId = null)
        {
            var clientActions = _clientApiService.GetNextRegisteredActionsToday(employeeId);

            var clientsWithNextAction = new List<ClientWithNextActionDTO>();
            foreach (var clientAction in clientActions)
            {
                var dto = new ClientWithNextActionDTO();
                dto.ClientId = clientAction.ClientId;
                dto.NextAction = MapRegisteredActionToBasicDto(clientAction);

                clientsWithNextAction.Add(dto);
            }

            return clientsWithNextAction.ToArray();
        }

        [HttpGet]
        [Route("all-clients-next-actions")]
        public ClientWithNextActionDTO[] GetAllClientsWithNextAction(Guid? employeeId = null)
        {
            var clientActions = _clientApiService.GetAllNextRegisteredActions(employeeId);
            var clients = _clientApiService.GetAgreedActionsLinkedClients(employeeId);

            var clientsWithNextAction = new List<ClientWithNextActionDTO>();
            foreach (var client in clients)
            {
                var nextClientAction = clientActions.SingleOrDefault(ca => ca.ClientId == client.Id);
                var dto = new ClientWithNextActionDTO();
                dto.ClientId = client.Id;
                dto.NextAction = nextClientAction != null ? MapRegisteredActionToBasicDto(nextClientAction) : null;

                clientsWithNextAction.Add(dto);
            }

            return clientsWithNextAction.ToArray();
        }

        // mappers
        private ClientDTO MapClientToDto(Client client)
        {
            var dto = new ClientDTO();
            dto.Id = client.Id;
            dto.FirstName = client.FirstName;
            dto.Surname = client.Surname;
            dto.FullName = client.FullName;
            dto.Gender = client.Gender;
            dto.BirthDate = client.Birthdate;
            dto.ProfilePicture = new PictureDTO { PictureUri = FormatPictureToBase64(client.ProfilePicture) };
            dto.Address = MapAddressToDto(client.Address);
            dto.Email = client.Email;
            dto.PhoneNumber = client.PhoneNumber;

            return dto;
        }

        private Client MapDtoToClient(ClientDTO clientDto)
        {
            var client = new Client();
            client.Id = clientDto.Id;
            client.FirstName = clientDto.FirstName;
            client.Surname = clientDto.Surname;
            client.PhoneNumber = clientDto.PhoneNumber;
            client.Email = clientDto.Email;
            client.Birthdate = clientDto.BirthDate;
            client.Gender = clientDto.Gender;


            client.Address = new Address();
            client.Address.City = clientDto.Address.City;
            client.Address.BuildingNumber = clientDto.Address.BuildingNumber;
            client.Address.PostCode = clientDto.Address.PostCode;
            client.Address.Street = clientDto.Address.Street;

            if (clientDto.ProfilePicture.PictureUri != null)
            {
                var picture = Base64ImageHelper.CreatePictureFromUri(clientDto.ProfilePicture.PictureUri);
                client.ProfilePicture = picture;
            }

            client.Address.Coordinates = new Coordinates();
            client.Address.Coordinates.Latitude = clientDto.Address.Coordinates.Latitude.ToString();
            client.Address.Coordinates.Longitude = clientDto.Address.Coordinates.Longitude.ToString();

            return client;
        }

        private AddressDTO MapAddressToDto(Address address)
        {
            var dto = new AddressDTO();
            dto.Id = address.Id;
            dto.BuildingNumber = address.BuildingNumber;
            dto.City = address.City;

            if (address.CoordinatesId.HasValue)
            {
                dto.Coordinates = new CoordinatesDTO();
                dto.Coordinates.Latitude = Convert.ToDecimal(address.Coordinates.Latitude);
                dto.Coordinates.Longitude = Convert.ToDecimal(address.Coordinates.Longitude);
            }

            dto.PostCode = address.PostCode;
            dto.Street = address.Street;

            return dto;
        }

        private IndividualPlanDTO MapIndividualPlanWithActionsToDto(IndividualPlan plan, List<AgreedClientAction> actions)
        {
            var actionsForDays = new List<AgreedActionsForDayDTO>();

            var actionsGrouped = actions.GroupBy(a => a.Day);

            foreach (var group in actionsGrouped)
            {
                var day = group.Key;
                var actionsForDay = new AgreedActionsForDayDTO();
                actionsForDay.Day = day;

                var actionsDtos = new List<AgreedActionDTO>();
                foreach (var action in group)
                {
                    var agreedActionDto = new AgreedActionDTO();
                    agreedActionDto.Id = action.Id;
                    agreedActionDto.EstimatedDurationMinutes = action.EstimatedDurationMinutes;
                    agreedActionDto.ClientActionSpecificDescription = action.ClientActionSpecificDescription;
                    agreedActionDto.PlannedStartTime = DateTime.Today.Add(action.PlannedStartTime);
                    agreedActionDto.PlannedEndTime = DateTime.Today.Add(action.PlannedStartTime.Add(TimeSpan.FromMinutes(action.EstimatedDurationMinutes)));

                    var actionDto = new ActionDTO();
                    actionDto.Id = action.ActionId;
                    actionDto.Description = action.Action.GeneralDescription;
                    actionDto.Name = action.Action.Name;

                    agreedActionDto.Action = actionDto;
                    actionsDtos.Add(agreedActionDto);
                }

                actionsDtos = actionsDtos.OrderBy(a => a.PlannedStartTime).ToList();
                actionsForDay.AgreedActions = actionsDtos.ToArray();
                actionsForDays.Add(actionsForDay);
            }

            var planDto = new IndividualPlanDTO();
            planDto.Id = plan.Id;
            planDto.ValidFrom = plan.ValidFromDate;
            planDto.ValidUntil = plan.ValidUntilDate;
            planDto.ActionsForDay = actionsForDays.ToArray();

            return planDto;
        }

        private RegisteredActionBasicDTO MapRegisteredActionToBasicDto(RegisteredClientAction action)
        {
            var dto = new RegisteredActionBasicDTO();
            dto.Id = action.Id;
            dto.Name = action.AgreedClientAction.Action.Name;
            dto.Date = action.PlannedStartDateTime;
            dto.Time = action.PlannedStartDateTime;

            return dto;
        }
    }
}