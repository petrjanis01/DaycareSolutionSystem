import { Component, OnInit } from '@angular/core';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { EmployeeService, EmployeeDetailDTO, PictureDTO } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {
  public employeeDetail: EmployeeDetailDTO;

  constructor(
    private imageHelper: ImageHelperService,
    private employeeService: EmployeeService,
    private auth: AuthenticationService,
    private toasterService: ToastService) { }

  ngOnInit() {
    this.loadEmployeeProfile();
  }

  private async loadEmployeeProfile() {
    this.employeeDetail = await this.employeeService.apiEmployeeGetEmployeeDetailGet();

    if (this.employeeDetail.profilePictureUri == null) {
      this.employeeDetail.profilePictureUri = './../../../assets/img/user-anonymous.png';
    }
  }

  public async changePhoto() {
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

  public changePassword() {
  }
}
