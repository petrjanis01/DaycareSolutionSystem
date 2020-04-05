import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientDTO, ClientsService, AddressDTO, CoordinatesDTO, PictureDTO } from 'src/app/api/generated';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { GeolocationHelperService } from 'src/app/services/geolocation-helper.service';
import { trigger, transition, query, style, animate, group } from '@angular/animations';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';
import { DatepickerDateModel } from '../datepicker-date-model';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

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
  selector: 'app-client-general-info',
  templateUrl: './client-general-info.component.html',
  styleUrls: ['./client-general-info.component.scss'],
  animations: [
    trigger('detailEditSlider', [
      transition(':increment', right),
      transition(':decrement', left),
    ]),
  ]
})
export class ClientGeneralInfoComponent implements OnInit {
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  @Input() client: ClientDTO;
  @Input() address: AddressDTO;

  public detailEditCounter = -1;

  public latClient: number;
  public lngClient: number;

  public datepickerBirthdateModel: DatepickerDateModel;

  public latUpdate: number;
  public lngUpdate: number;

  public updateDetailForm: FormGroup;

  private isEdit = false;

  constructor(private clientsService: ClientsService, public helper: GeneralHelperService,
    private geolocationHelper: GeolocationHelperService, private formBuilder: FormBuilder,
    private router: Router, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    if (this.client.profilePicture.pictureUri == null) {
      this.client.profilePicture.pictureUri = './../../../../../assets/img/user-anonymous.png';
    }

    if (this.client.id == null) {
      this.openGeneralInfoEdit();
      this.detailEditCounter = 1;
      console.log('adasda');
      return;
    }

    this.isEdit = true;
    this.detailEditCounter = 0;

    this.latClient = this.address.coordinates.latitude;
    this.lngClient = this.address.coordinates.longitude;
  }

  public async openGeneralInfoEdit() {
    this.createDetailEditFormGroup();

    if (this.isEdit) {
      this.latUpdate = this.latClient;
      this.lngUpdate = this.lngClient;
    } else {
      let cords = this.geolocationHelper.getCurrentLocation();
      if (cords == null) {
        this.latUpdate = 0;
        this.lngUpdate = 0;
      }
    }

    this.detailEditCounter++;
  }

  public async saveGeneralInfo() {
    await this.validateAddress();
    let updateDto = this.createUpdateDto();

    this.spinner.show();
    try {
      let client: ClientDTO;
      if (this.isEdit) {
        client = await this.clientsService.apiClientsSingleClientPut(updateDto);
      } else {
        client = await this.clientsService.apiClientsSingleClientPost(updateDto);
      }

      this.client = client;
      this.address = client.address;
      this.latClient = client.address.coordinates.latitude;
      this.lngClient = client.address.coordinates.longitude;
    } catch{
      this.client = updateDto;
      await this.geolocationHelper.getFullAddressIfNeeded(updateDto.address);
      this.address = updateDto.address;
    } finally {
      this.spinner.hide();
    }

    this.detailEditCounter--;
  }

  private createUpdateDto(): ClientDTO {
    let formValue = this.updateDetailForm.value;
    let coordinatesUpdateDto: CoordinatesDTO = {
      latitude: this.latUpdate,
      longitude: this.lngUpdate
    };

    let addressUpdateDto: AddressDTO = {
      id: this.isEdit ? this.address.id : null,
      postCode: formValue.postCode,
      city: formValue.city,
      street: formValue.street,
      buildingNumber: formValue.buildingNumber,
      coordinates: coordinatesUpdateDto
    };

    let profilePictureUpdateDto: PictureDTO = {
      pictureUri: this.client.profilePicture.pictureUri
    };

    let birthDate: DatepickerDateModel = formValue.birthDate;
    let date = new Date();
    date.setFullYear(birthDate.year);
    date.setMonth(birthDate.month);
    date.setDate(birthDate.day);

    let clientUpdateDto: ClientDTO = {
      id: this.isEdit ? this.client.id : null,
      firstName: formValue.firstName,
      surname: formValue.surname,
      email: formValue.email,
      phoneNumber: formValue.phoneNumber,
      birthDate: date,
      gender: formValue.gender,
      profilePicture: profilePictureUpdateDto,
      address: addressUpdateDto
    };

    console.log(clientUpdateDto);
    return clientUpdateDto;
  }

  public cancelEdit() {
    if (!this.isEdit) {
      this.router.navigate(['clients']);
      return;
    }

    this.detailEditCounter--;
    console.log(this.detailEditCounter);
    this.updateDetailForm = null;
  }

  public selectImage(ev: any) {
    this.imageInput.nativeElement.click();
  }

  public async validateAddress() {
    let address = this.createAddressFromFormValues();
    let res = await this.geolocationHelper.getGpsCoordinatesFromAddress(address);
    this.latUpdate = res.latitude;
    this.lngUpdate = res.longitude;
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
    let date = this.client.birthDate ? this.client.birthDate : new Date();
    this.datepickerBirthdateModel = new DatepickerDateModel(date);

    this.updateDetailForm = this.formBuilder.group({
      firstName: new FormControl(this.client.firstName, Validators.required),
      surname: new FormControl(this.client.surname, Validators.required),
      email: new FormControl(this.client.email, Validators.required),
      phoneNumber: new FormControl(this.client.phoneNumber, Validators.required),
      birthDate: new FormControl(new DatepickerDateModel(date), Validators.required),
      gender: new FormControl(this.client.gender, Validators.required),
      city: new FormControl(this.address.city, Validators.required),
      postCode: new FormControl(this.address.postCode, Validators.required),
      street: new FormControl(this.address.street),
      buildingNumber: new FormControl(this.address.buildingNumber, Validators.required)
    });
  }

  // https://stackoverflow.com/questions/39272970/angular-2-encode-image-to-base64
  public processFile(ev: any) {
    let files = ev.target.files;
    let file: File = files[0];

    if (files && file) {
      let reader = new FileReader();

      this.imageToBase64(reader, file)
        .subscribe(base64image => {
          this.client.profilePicture.pictureUri = base64image;
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
