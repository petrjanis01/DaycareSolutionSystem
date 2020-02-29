import { Component, OnInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';
import { ClientsService } from 'src/app/api/generated';
import { GeolocationHelperService } from 'src/app/services/geolocation/geolocation-helper-service';
import { Address } from 'src/app/services/geolocation/address';
import { RegisteredActionBasicDTO } from 'src/app/api/generated/model/registeredActionBasicDTO';
import { ClientWithNextActionDTO } from 'src/app/api/generated/model/clientWithNextActionDTO';

@Component({
  selector: 'app-map',
  templateUrl: './map.page.html',
  styleUrls: ['./map.page.scss'],
})
export class MapPage implements OnInit {
  public clients: Client[];
  public nextClientActions: ClientWithNextActionDTO[];
  public mapStartLat: number;
  public mapStartLng: number;
  public displaySelfMarker: boolean;
  public displayedClient: Client;
  public displayedClientAction: RegisteredActionBasicDTO;

  constructor(
    private cache: ClientsCacheService,
    private clientsService: ClientsService,
    private geolocationHelper: GeolocationHelperService) { }

  async ngOnInit() {
    this.displaySelfMarker = true;
    await this.cache.loaded;
    await this.loadClientsForToday();
    this.getMapStartingPostion();
  }

  private async getMapStartingPostion() {
    let cords = await this.geolocationHelper.getCurrentLocation();
    if (cords == null && this.clients[0]) {
      cords = this.clients[0].address.coordinates;
      this.displaySelfMarker = false;
    }

    this.mapStartLat = +cords.latitude;
    this.mapStartLng = +cords.longitude;
  }

  private async loadClientsForToday() {
    let clientsWithNextAction = await this.clientsService.apiClientsTodayScheduledClientsGet();

    let clients = new Array<Client>();

    for (let clientWithNextAction of clientsWithNextAction) {
      let client = this.cache.getClientById(clientWithNextAction.clientId);
      await this.getCoordinatesIfNeeded(client);
      await this.getAddressIfNeeded(client);

      clients.push(client);
    }

    this.clients = clients;
    this.nextClientActions = clientsWithNextAction;
  }

  private async getAddressIfNeeded(client: Client) {
    let address = new Address(client.address);

    let isAddressComplete = address.city == null || address.buildingNumber == null || address.postCode == null;
    if (isAddressComplete) {
      let addressCalculated = await (await this.geolocationHelper.getAddressFromgGpsCoordinates(address.coordinates));
      client.address.buildingNumber = addressCalculated.buildingNumber;
      client.address.street = addressCalculated.street;
      client.address.postCode = addressCalculated.postCode;
      client.address.city = addressCalculated.city;
    }
  }

  private async getCoordinatesIfNeeded(client: Client) {
    let address = new Address(client.address);

    if (address.coordinates == null) {
      let coordinates = await (await this.geolocationHelper.getGpsCoordinatesFromAddress(address));
      client.address.coordinates = coordinates;
    }
  }

  clientOnMapClicked(id: string) {
    this.clients.forEach(client => {
      if (client.id === id) {
        this.displayedClient = client;
        return;
      }
    });

    this.nextClientActions.forEach(action => {
      if (action.clientId === id) {
        this.displayedClientAction = action.nextAction;
        return;
      }
    });
  }

  closeDetail() {
    this.displayedClient = null;
    this.displayedClientAction = null;
  }
}
