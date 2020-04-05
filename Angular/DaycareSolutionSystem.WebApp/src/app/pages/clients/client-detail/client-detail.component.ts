import { Component, OnInit, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientDTO, ClientsService, AddressDTO, CoordinatesDTO, PictureDTO } from 'src/app/api/generated';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { GeolocationHelperService } from 'src/app/services/geolocation-helper.service';
import { trigger, transition, query, style, animate, group } from '@angular/animations';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';
import { DatepickerDateModel } from './datepicker-date-model';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

const left = [
  query(':enter, :leave', style({ position: 'absolute' }), { optional: true }),
  group([
    query(':enter', [style({ transform: 'translateX(-100%)', opacity: 0 }),
    animate('1s ease-out', style({ transform: 'translateX(0%)', opacity: 1 }))], {
      optional: true,
    }),
    query(':leave', [style({ transform: 'translateX(0%)', opacity: 1 }),
    animate('1s ease-out', style({ transform: 'translateX(100%)', opacity: 0 }))], {
      optional: true,
    }),
  ]),
];

const right = [
  query(':enter, :leave', style({ position: 'absolute' }), { optional: true }),
  group([
    query(':enter', [style({ transform: 'translateX(100%)', opacity: 0 }),
    animate('1s ease-out', style({ transform: 'translateX(0%)', opacity: 1 }))], {
      optional: true,
    }),
    query(':leave', [style({ transform: 'translateX(0%)', opacity: 1 }),
    animate('1s ease-out', style({ transform: 'translateX(-100%)', opacity: 0 }))], {
      optional: true,
    }),
  ]),
];

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.scss'],
  animations: [
    trigger('detailEditSlider', [
      transition(':increment', right),
      transition(':decrement', left),
    ]),
  ]
})
export class ClientDetailComponent implements OnInit {
  public client: ClientDTO;
  public address: AddressDTO;

  constructor(private route: ActivatedRoute, private clientsService: ClientsService,
    public helper: GeneralHelperService, private geolocationHelper: GeolocationHelperService,
    private formBuilder: FormBuilder) { }

  async ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    if (id === '0') {
      this.createNewClientDto();
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
}
