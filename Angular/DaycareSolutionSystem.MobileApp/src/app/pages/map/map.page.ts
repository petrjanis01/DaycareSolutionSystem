import { Component, OnInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';
import { ClientsService } from 'src/app/api/generated';
import { GeolocationHelperService } from 'src/app/services/geolocation/geolocation-helper-service';
import { Address } from 'src/app/services/geolocation/address';
import { RegisteredActionBasicDTO } from 'src/app/api/generated/model/registeredActionBasicDTO';
import { ClientWithNextActionDTO } from 'src/app/api/generated/model/clientWithNextActionDTO';
import { PopoverController } from '@ionic/angular';
import { MapMenuComponent } from './map-menu/map-menu.component';

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
  public allClientsOnMap: boolean;
  public nearbyClientsCount: number;

  constructor(
    private cache: ClientsCacheService,
    private clientsService: ClientsService,
    private geolocationHelper: GeolocationHelperService,
    private popoverController: PopoverController) { }

  async ngOnInit() {
    this.displaySelfMarker = true;
    this.allClientsOnMap = false;

    await this.cache.loaded;
    await this.loadCLientsWithNextActions();
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

  private async loadCLientsWithNextActions() {
    let clientsWithNextAction;

    if (this.allClientsOnMap === false) {
      clientsWithNextAction = await this.clientsService.apiClientsTodayScheduledClientsGet();
    } else {
      clientsWithNextAction = await this.clientsService.apiClientsAllClientsNextActionsGet();
    }

    this.processClients(clientsWithNextAction);
  }

  private async processClients(clientsWithNextAction: ClientWithNextActionDTO[]) {
    let clients = new Array<Client>();

    clientsWithNextAction.forEach(clientWithNextAction => {
      let client = this.cache.getClientById(clientWithNextAction.clientId);
      clients.push(client);
    });

    this.clients = clients;
    this.nextClientActions = clientsWithNextAction;
    this.nearbyClientsCount = this.getNearbyClients().length;
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

  public navigateInExternalApp() {
    console.log('open external app');
  }

  public async presentPopover(ev: any) {
    let nearbyClients = this.getNearbyClients();

    let popover = await this.popoverController.create({
      component: MapMenuComponent,
      componentProps: {
        allClientsOnMap: this.allClientsOnMap,
        hasDeviceLocation: this.displaySelfMarker,
        nearbyClients
      },
      event: ev,
      translucent: true,
    });
    popover.present();

    popover.onDidDismiss().then(
      (data: any) => {
        if (data && data.data) {
          this.allClientsOnMap = data.data.allClientsOnMap;
          this.loadCLientsWithNextActions();
        }
      });
  }

  private getNearbyClients(): Client[] {
    let nearbyClients = new Array<Client>();
    this.clients.forEach(client => {
      if (client.distanceFromDevice < 1000) {
        nearbyClients.push(client);
      }
    });

    return nearbyClients;
  }
}
