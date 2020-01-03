using System;
using System.Collections.Generic;
using DaycareSolutionSystem.Api.Host.Controllers.DTO;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Database.Entities.Entities;
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
        [Route("single-client")]
        public ClientDTO GetClient(Guid clientId)
        {
            var client = _clientApiService.GetClient(clientId);
            var dto = MapClientToDto(client);

            return dto;
        }

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

            return dto;
        }

        private AddressDTO MapAddressToDto(Address address)
        {
            var dto = new AddressDTO();
            dto.Id = address.Id;
            dto.BuildingNumber = address.BuildingNumber;
            dto.City = address.City;
            dto.GpsCoordinates = address.GpsCoordinates;
            dto.PostCode = address.PostCode;
            dto.Street = address.Street;

            return dto;
        }
    }
}