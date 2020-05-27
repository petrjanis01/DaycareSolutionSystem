import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { ClientDTO, ClientsService, AddressDTO, CoordinatesDTO, PictureDTO } from 'src/app/api/generated';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { GeolocationHelperService } from 'src/app/services/geolocation-helper.service';
import { trigger, transition } from '@angular/animations';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';
import { DatepickerDateModel } from '../../../../shared/datepicker-date-model';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SliderAnimation } from 'src/app/shared/slider-animation'
import { NotifiactionService } from 'src/app/services/notification.service';


@Component({
  selector: 'app-client-general-info',
  templateUrl: './client-general-info.component.html',
  styleUrls: ['./client-general-info.component.scss'],
  animations: [
    trigger('detailEditSlider', [
      transition(':increment', SliderAnimation.right),
      transition(':decrement', SliderAnimation.left),
    ]),
  ]
})
export class ClientGeneralInfoComponent implements OnInit {
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  @Input() client: ClientDTO;
  @Input() address: AddressDTO;
  @Output() onanimationInProgress = new EventEmitter();

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
    private router: Router, private spinner: NgxSpinnerService,
    private notifications: NotifiactionService) { }

  ngOnInit() {
    if (this.client.profilePicture.pictureUri == null) {
      this.client.profilePicture.pictureUri = this.helper.getAnonymousImgUrlFormatted();
    }

    if (this.client.id == null) {
      this.openGeneralInfoEdit();
      this.detailEditCounter = 1;
      return;
    }

    this.isEdit = true;
    this.detailEditCounter = 0;

    this.latClient = this.address.coordinates.latitude;
    this.lngClient = this.address.coordinates.longitude;
  }

  getTitle(): string {
    if (this.isEdit) {
      return 'Client edit';
    }

    return 'Create client'
  }

  public async openGeneralInfoEdit() {
    this.createDetailEditFormGroup();

    if (this.isEdit) {
      this.latUpdate = this.latClient;
      this.lngUpdate = this.lngClient;

      this.detailEditCounter++;
      this.onanimationInProgress.emit();
    } else {
      let cords = await this.geolocationHelper.getCurrentLocation();
      if (cords == null) {
        this.latUpdate = 0;
        this.lngUpdate = 0;
      } else {
        this.latUpdate = cords.latitude;
        this.lngUpdate = cords.longitude;

        this.latClient = this.latUpdate;
        this.lngClient = this.lngUpdate;
      }
    }
  }

  public async saveGeneralInfo() {
    await this.validateAddress();
    let updateDto = this.createUpdateDto();
    let client: ClientDTO;
    this.spinner.show();
    try {
      if (this.isEdit) {
        client = await this.clientsService.apiClientsSingleClientPut(updateDto);
      } else {
        console.log(updateDto);
        client = await this.clientsService.apiClientsSingleClientPost(updateDto);
      }

      this.notifications.showSuccessNotification('Operation succesful');
      this.client = client;
      this.address = client.address;
      this.latClient = client.address.coordinates.latitude;
      this.lngClient = client.address.coordinates.longitude;
      this.isEdit = true;
      this.onanimationInProgress.emit();
      this.detailEditCounter--;
    } catch{
      this.client = updateDto;
      await this.geolocationHelper.getFullAddressIfNeeded(updateDto.address);
      this.address = updateDto.address;
    } finally {
      this.spinner.hide();

      if (this.client.profilePicture.pictureUri == null) {
        this.client.profilePicture.pictureUri = this.helper.getAnonymousImgUrlFormatted();
      }
    }
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
      pictureUri: this.client.profilePicture.pictureUri.includes(this.helper.getAnonymousImgUrlFormatted()) ? null :
        this.client.profilePicture.pictureUri
    };

    let birthDateModel: DatepickerDateModel = formValue.birthDate;
    let date = this.helper.getDateFromDatepickerModel(birthDateModel);
    let clientUpdateDto: ClientDTO = {
      id: this.isEdit ? this.client.id : null,
      firstName: formValue.firstName,
      surname: formValue.surname,
      email: formValue.email,
      phoneNumber: formValue.phoneNumber,
      birthDate: date,
      gender: formValue.gender,
      profilePicture: profilePictureUpdateDto,
      address: addressUpdateDto,
    };

    return clientUpdateDto;
  }

  public cancelEdit() {
    if (!this.isEdit) {
      this.router.navigate(['clients']);
      return;
    }

    this.onanimationInProgress.emit();
    this.detailEditCounter--;
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
      email: new FormControl(this.client.email, Validators.email),
      phoneNumber: new FormControl(this.client.phoneNumber),
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
