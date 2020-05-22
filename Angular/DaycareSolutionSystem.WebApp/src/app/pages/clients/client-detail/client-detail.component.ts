import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientDTO, ClientsService, AddressDTO, CoordinatesDTO, PictureDTO } from 'src/app/api/generated';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { GeolocationHelperService } from 'src/app/services/geolocation-helper.service';
import { trigger, transition } from '@angular/animations';
import { SliderAnimation } from 'src/app/shared/slider-animation'

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.scss'],
  animations: [
    trigger('detailEditSlider', [
      transition(':increment', SliderAnimation.right),
      transition(':decrement', SliderAnimation.left),
    ]),
  ]
})
export class ClientDetailComponent implements OnInit {
  public client: ClientDTO;
  public address: AddressDTO;
  public isIndividualPlanCardVisible = true;

  constructor(private route: ActivatedRoute, private clientsService: ClientsService,
    public helper: GeneralHelperService, private geolocationHelper: GeolocationHelperService) { }

  async ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    if (id === '0') {
      this.createNewClientDto();
      this.isIndividualPlanCardVisible = false;
    } else {
      this.getExistingClient(id);
    }

  }

  private async getExistingClient(id: string) {
    let dto = await this.clientsService.apiClientsSingleClientGet(id);
    let address = dto.address;
    await this.geolocationHelper.getFullAddressIfNeeded(address);
    this.address = address;
    this.client = dto;
  }

  private createNewClientDto() {

    let coordinatesEmptyDto: CoordinatesDTO = {
      latitude: null,
      longitude: null
    };

    let addressEmptyDto: AddressDTO = {
      id: null,
      postCode: null,
      city: null,
      street: null,
      buildingNumber: null,
      coordinates: coordinatesEmptyDto
    };

    let profilePictureEmptyDto: PictureDTO = {
      pictureUri: null
    };

    let clientEmptyDto: ClientDTO = {
      id: null,
      firstName: null,
      surname: null,
      email: null,
      phoneNumber: null,
      birthDate: null,
      gender: null,
      profilePicture: profilePictureEmptyDto,
      address: addressEmptyDto
    };
    this.address = addressEmptyDto;
    this.client = clientEmptyDto;
  }

  public onClientInfoAnimationInProgress() {
    this.isIndividualPlanCardVisible = false;

    setTimeout(() => { this.isIndividualPlanCardVisible = true; }, 1200);
  }
}
