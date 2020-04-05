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
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  public detailEditCounter = 0;

  public client: ClientDTO;
  public address: AddressDTO;
  public latClient: number;
  public lngClient: number;

  public newProfilePicture: string;
  public clientUpdateDto: ClientDTO;
  public datepickerBirthdateModel: DatepickerDateModel;

  public latUpdate: number;
  public lngUpdate: number;

  public updateDetailForm: FormGroup;

  constructor(private route: ActivatedRoute, private clientsService: ClientsService,
    public helper: GeneralHelperService, private geolocationHelper: GeolocationHelperService,
    private formBuilder: FormBuilder) { }

  async ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    let dto = await this.clientsService.apiClientsSingleClientGet(id);
    if (dto.profilePicture.pictureUri == null) {
      dto.profilePicture.pictureUri = './../../../../assets/img/user-anonymous.png';
    }

    this.client = dto;
    let address = this.client.address;
    await this.geolocationHelper.getFullAddressIfNeeded(address);
    this.address = address;
    this.latClient = address.coordinates.latitude;
    this.lngClient = address.coordinates.longitude;
  }

  public async openGeneralInfoEdit() {
    this.createDetailEditFormGroup();
    this.mapClientToUpdateDTO();

    this.latUpdate = this.latClient;
    this.lngUpdate = this.lngClient;
    // // when creating new client
    // let cords = await this.geolocationHelper.getCurrentLocation();
    // if (cords != null) {
    //   this.latUpdate = cords.latitude;
    //   this.lngUpdate = cords.longitude;
    // }

    // this.updateMapFromForm();

    this.detailEditCounter++;
  }

  public saveGeneralInfo() {
    this.detailEditCounter--;
  }

  public selectImage(ev: any) {
    this.imageInput.nativeElement.click();
  }

  public confirmAddress() {
    let address = this.createAddressFromFormValues();
    this.geolocationHelper.getGpsCoordinatesFromAddress(address).then(res => {
      this.latUpdate = res.latitude;
      this.lngUpdate = res.longitude;
    });
  }

  public allAddressControlsValid(): boolean {
    let controlNames: Array<string> = ['city', 'postCode', 'street', 'buildingNumber'];
    let allValid = true;

    for (let controlName of controlNames) {
      let isValid = this.updateDetailForm.controls[controlName].valid;
      allValid = allValid && isValid;
    }

    return allValid;
  }

  private createAddressFromFormValues(): AddressDTO {
    let address: AddressDTO = {
      city: this.updateDetailForm.controls.city.value,
      postCode: this.updateDetailForm.controls.postCode.value,
      street: this.updateDetailForm.controls.street.value,
      buildingNumber: this.updateDetailForm.controls.buildingNumber.value,
    };

    return address;
  }

  private createDetailEditFormGroup() {
    this.updateDetailForm = this.formBuilder.group({
      firstName: new FormControl(this.client.firstName, Validators.required),
      surname: new FormControl(this.client.surname, Validators.required),
      email: new FormControl(this.client.email, Validators.required),
      phoneNumber: new FormControl(this.client.phoneNumber, Validators.required),
      birthDate: new FormControl(new DatepickerDateModel(this.client.birthDate), Validators.required),
      gender: new FormControl(this.client.gender, Validators.required),
      city: new FormControl(this.address.city, Validators.required),
      postCode: new FormControl(this.address.postCode, Validators.required),
      street: new FormControl(this.address.street),
      buildingNumber: new FormControl(this.address.buildingNumber, Validators.required)
    });
  }

  private mapClientToUpdateDTO() {
    let coordinatesUpdateDto: CoordinatesDTO = {
      latitude: this.client.address.coordinates.latitude,
      longitude: this.client.address.coordinates.longitude
    };

    let addressUpdateDto: AddressDTO = {
      id: this.client.address.id,
      postCode: this.client.address.postCode,
      city: this.client.address.city,
      street: this.client.address.street,
      buildingNumber: this.client.address.buildingNumber,
      coordinates: coordinatesUpdateDto
    };

    let profilePictureUpdateDto: PictureDTO = {
      pictureUri: this.client.profilePicture.pictureUri
    };

    let clientUpdateDto: ClientDTO = {
      id: this.client.id,
      firstName: this.client.firstName,
      surname: this.client.surname,
      email: this.client.email,
      phoneNumber: this.client.phoneNumber,
      birthDate: this.client.birthDate,
      gender: this.client.gender,
      profilePicture: profilePictureUpdateDto,
      address: addressUpdateDto
    };

    this.datepickerBirthdateModel = new DatepickerDateModel(this.client.birthDate);
    this.clientUpdateDto = clientUpdateDto;
  }

  // https://stackoverflow.com/questions/39272970/angular-2-encode-image-to-base64
  public processFile(ev: any) {
    let files = ev.target.files;
    let file: File = files[0];

    if (files && file) {
      let reader = new FileReader();

      this.imageToBase64(reader, file)
        .subscribe(base64image => {
          this.clientUpdateDto.profilePicture.pictureUri = base64image;
        });
    }
  }

  private imageToBase64(fileReader: FileReader, fileToRead: File): Observable<string> {
    fileReader.readAsDataURL(fileToRead);
    return fromEvent(fileReader, 'load').pipe(pluck('currentTarget', 'result'));
  }

  public async mapClicked(ev: any) {
    if (ev && ev.coords) {
      this.latUpdate = ev.coords.lat;
      this.lngUpdate = ev.coords.lng;

      let cords: CoordinatesDTO = {
        latitude: this.latUpdate,
        longitude: this.lngUpdate
      }

      let address = await this.geolocationHelper.getAddressFromgGpsCoordinates(cords);
      this.updateDetailForm.controls.city.setValue(address.city);
      this.updateDetailForm.controls.postCode.setValue(address.postCode);
      this.updateDetailForm.controls.street.setValue(address.street);
      this.updateDetailForm.controls.buildingNumber.setValue(address.buildingNumber);
    }
  }

}
