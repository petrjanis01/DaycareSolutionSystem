import { Component, OnInit } from '@angular/core';
import { Client } from 'src/app/services/clients/client';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { ActivatedRoute } from '@angular/router';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { ClientsService, PictureDTO } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { GeolocationHelperService } from 'src/app/services/geolocation/geolocation-helper-service';
import { Address } from 'src/app/services/geolocation/address';
import { IndividualPlanDTO } from 'src/app/api/generated/model/individualPlanDTO';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.page.html',
  styleUrls: ['./client-detail.page.scss'],
})
export class ClientDetailPage implements OnInit {
  public client: Client;
  public lat: number;
  public lng: number;
  public individualPlans: IndividualPlanDTO[];

  constructor(
    private clientCache: ClientsCacheService,
    private activatedRoute: ActivatedRoute,
    private imageHelper: ImageHelperService,
    private clientsService: ClientsService,
    private toaster: ToastService,
    public generalHelper: GeneralHelperService,
    private geolocationHelper: GeolocationHelperService
  ) { }

  async ngOnInit() {
    let id = this.activatedRoute.snapshot.paramMap.get('id');
    this.client = await this.clientCache.getClientById(id);

    this.getCoordinatesForMapIfNeeded();
    this.getIndividualPlans();
    this.getAddressIfNeeded();
  }

  private async getIndividualPlans() {
    let plans = await this.clientsService.apiClientsAgreedActionsByPlansGet(this.client.id);

    this.individualPlans = plans;
  }

  private async getAddressIfNeeded() {
    let address = new Address(this.client.address);

    if ((address.buildingNumber == null || address.city == null || address.postCode == null)
      && address.gpsCoordinates != null) {
      let coordinates = address.gpsCoordinates;
      let addressCalculated = await this.geolocationHelper.getAddressFromgGpsCoordinates(coordinates);

      this.client.address = addressCalculated;
      this.client.address.gpsCoordinates = coordinates;
    }
  }

  private async getCoordinatesForMapIfNeeded() {
    let address = new Address(this.client.address);
    if (address.gpsCoordinates == null) {
      let coordinates = await (await this.geolocationHelper.getGpsCoordinatesFromAddress(address)).split(',');
      this.lat = Number(coordinates[0]);
      this.lng = Number(coordinates[1]);
    } else {
      let coordinates = address.gpsCoordinates.split(',');
      this.lat = Number(coordinates[0]);
      this.lng = Number(coordinates[1]);
    }
  }

  public async changeClientProfilePicture() {
    let img = await this.imageHelper.getImageAsBase64FromDevice();

    if (img != null) {
      let dto: PictureDTO = {
        pictureUri: img
      };

      this.clientsService.apiClientsChangeProfilePicturePost(this.client.id, dto)
        .then(async () => {
          let client = await this.clientCache.reloadSingleClient(this.client.id);
          this.client = client;
        })
        .catch(() => this.toaster.showErrorToast('Changing image failed'));
    }
  }
}
