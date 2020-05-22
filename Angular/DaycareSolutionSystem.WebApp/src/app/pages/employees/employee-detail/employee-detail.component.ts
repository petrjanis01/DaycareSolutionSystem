import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { EmployeeService, EmployeeDetailDTO } from 'src/app/api/generated';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SliderAnimation } from 'src/app/shared/slider-animation';
import { trigger, transition } from '@angular/animations';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';
import { DatepickerDateModel } from 'src/app/shared/datepicker-date-model';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotifiactionService } from 'src/app/services/notification.service';
import { UserDTO } from 'src/app/api/generated/model/userDTO';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
  animations: [
    trigger('detailEditSlider', [
      transition(':increment', SliderAnimation.right),
      transition(':decrement', SliderAnimation.left),
    ]),
  ]
})
export class EmployeeDetailComponent implements OnInit {
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  public employee: EmployeeDetailDTO;
  public isEdit: boolean

  public editForm: FormGroup;
  public detailEditCounter = -1;

  public profilePicture: string;

  public userForm: FormGroup;

  constructor(private employeeService: EmployeeService, private route: ActivatedRoute,
    private formBuilder: FormBuilder, public helper: GeneralHelperService,
    private router: Router, private spinner: NgxSpinnerService,
    private notifications: NotifiactionService) { }

  ngOnInit() {
    this.reload();
  }

  private async reload() {
    let id = this.route.snapshot.paramMap.get('id');
    console.log(id);
    this.isEdit = id !== '0';
    this.detailEditCounter = 0;

    if (this.isEdit) {
      this.employee = await this.employeeService.apiEmployeeGetEmployeeDetailGet(id);
      if (this.employee.profilePictureUri == null) {
        this.employee.profilePictureUri = this.helper.getAnonymousImgUrlFormatted();
      }
    } else {
      this.openGeneralInfoEdit();
    }
  }

  public openGeneralInfoEdit() {
    this.createEditForm();
    this.createUserForm();
    this.profilePicture = this.isEdit ? this.employee.profilePictureUri : this.helper.getAnonymousImgUrlFormatted();
    this.detailEditCounter++;
  }

  private createEditForm() {
    this.editForm = this.formBuilder.group({
      firstName: new FormControl(this.isEdit ? this.employee.firstName : '', Validators.required),
      surname: new FormControl(this.isEdit ? this.employee.surname : '', Validators.required),
      gender: new FormControl(this.isEdit ? this.employee.gender : 1, Validators.required),
      birthDate: new FormControl(new DatepickerDateModel(this.isEdit ? this.employee.birthdate : new Date()), Validators.required),
      phoneNumber: new FormControl(this.isEdit ? this.employee.phoneNumber : ''),
      email: new FormControl(this.isEdit ? this.employee.email : '', Validators.email),
      position: new FormControl(this.isEdit ? this.employee.employeePosition : 0),
    });
  }

  private createUserForm() {
    if (this.isEdit) {
      return;
    }

    this.userForm = this.formBuilder.group({
      loginName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  public cancelEdit() {
    if (!this.isEdit) {
      this.router.navigate(['employees']);
      return;
    }

    this.detailEditCounter--;
    this.editForm = null;
  }

  public async saveGeneralInfo() {
    this.spinner.show();
    let dto = this.createEmployeeUpdateDto();
    let employee: EmployeeDetailDTO;
    try {
      if (this.isEdit) {
        employee = await this.employeeService.apiEmployeePut(dto);
      } else {
        employee = await this.employeeService.apiEmployeePost(dto);
      }

      if (employee) {
        this.notifications.showSuccessNotification('Operation sucessful');

        if (this.isEdit) {
          this.reload();
        } else {
          this.employee = employee;
          this.router.navigate(['employees/detail', employee.id]);
          if (this.employee.profilePictureUri === null) {
            this.employee.profilePictureUri = this.helper.getAnonymousImgUrlFormatted();
          }
          this.isEdit = true;
          this.detailEditCounter--;
        }
      }
    } finally {
      this.spinner.hide();
    }

  }

  private createEmployeeUpdateDto(): EmployeeDetailDTO {
    let formValue = this.editForm.value;

    let model: DatepickerDateModel = formValue.birthDate;
    let bithDate = this.helper.getDateFromDatepickerModel(model);

    let dto: EmployeeDetailDTO = {};
    dto.birthdate = bithDate;
    dto.firstName = formValue.firstName;
    dto.surname = formValue.surname;
    dto.email = formValue.email;
    dto.phoneNumber = formValue.phoneNumber;
    dto.employeePosition = formValue.position;
    dto.gender = formValue.gender;
    dto.id = this.isEdit ? this.employee.id : null;
    dto.profilePictureUri = this.profilePicture.includes(this.helper.getAnonymousImgUrlFormatted()) ? null : this.profilePicture;

    if (this.isEdit === false) {
      let userDto: UserDTO = {};
      userDto.loginName = this.userForm.value.loginName;
      userDto.password = this.userForm.value.password;
      dto.user = userDto;
    }

    return dto;
  }

  public processFile(ev: any) {
    let files = ev.target.files;
    let file: File = files[0];

    if (files && file) {
      let reader = new FileReader();

      this.imageToBase64(reader, file)
        .subscribe(base64image => {
          this.profilePicture = base64image;
        });
    }
  }

  private imageToBase64(fileReader: FileReader, fileToRead: File): Observable<string> {
    fileReader.readAsDataURL(fileToRead);
    return fromEvent(fileReader, 'load').pipe(pluck('currentTarget', 'result'));
  }

  public selectImage(ev: any) {
    this.imageInput.nativeElement.click();
  }

  public isFormValid(): boolean {
    if (this.isEdit) {
      return this.editForm.valid;
    }

    return this.editForm.valid && this.userForm.valid;
  }

}
