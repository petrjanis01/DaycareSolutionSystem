import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { EmployeeService, EmployeeDetailDTO, PictureDTO } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { VisualHelperService } from 'src/app/services/visual-helper.service';
import { Platform } from '@ionic/angular';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {
  public employeeDetail: EmployeeDetailDTO;
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  constructor(
    private imageHelper: ImageHelperService,
    private employeeService: EmployeeService,
    private auth: AuthenticationService,
    private toasterService: ToastService,
    private helper: GeneralHelperService,
    public visualHelper: VisualHelperService,
    private platform: Platform) { }

  ngOnInit() {
    this.loadEmployeeProfile();
  }

  private async loadEmployeeProfile() {
    this.employeeDetail = await this.employeeService.apiEmployeeGetEmployeeDetailGet();

    if (this.employeeDetail.profilePictureUri == null) {
      this.employeeDetail.profilePictureUri = this.helper.getAnonymousImgUrlFormatted();
    }
  }

  public async changePhoto() {
    if (this.platform.is('capacitor') === false) {
      this.imageInput.nativeElement.click();
      return;
    }

    let image = await this.imageHelper.getImageAsBase64FromDevice();

    if (image != null) {
      let dto: PictureDTO = {
        pictureUri: image
      };

      this.employeeService.apiEmployeeChangeProfilePicturePost(null, dto)
        .then(() => this.loadEmployeeProfile())
        .catch(() => this.toasterService.showErrorToast('Changing image failed'));
    }
  }

  public logout() {
    this.auth.logOut();
  }

  public processFile(ev: any) {
    let files = ev.target.files;
    let file: File = files[0];

    if (files && file) {
      let reader = new FileReader();

      this.imageToBase64(reader, file)
        .subscribe(base64image => {

          if (base64image != null) {
            let dto: PictureDTO = {
              pictureUri: base64image
            };

            this.employeeService.apiEmployeeChangeProfilePicturePost(null, dto)
              .then(() => this.loadEmployeeProfile())
              .catch(() => this.toasterService.showErrorToast('Changing image failed'));
          }
        });
    }
  }

  private imageToBase64(fileReader: FileReader, fileToRead: File): Observable<string> {
    fileReader.readAsDataURL(fileToRead);
    return fromEvent(fileReader, 'load').pipe(pluck('currentTarget', 'result'));
  }

  // public changePassword() {
  // }
}
