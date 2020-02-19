import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { RegisteredActionDTO, RegisteredActionsService } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-client-action-detail',
  templateUrl: './client-action-detail.page.html',
  styleUrls: ['./client-action-detail.page.scss'],
})
export class ClientActionDetailPage implements OnInit {
  @Input() action: RegisteredActionDTO;

  public actionEditable: RegisteredActionDTO;

  constructor(
    private modalController: ModalController,
    private registeredActionsService: RegisteredActionsService,
    private toaster: ToastService,
    private imageHelper: ImageHelperService,
    public generalHelper: GeneralHelperService
  ) { }

  ngOnInit() {
    this.actionEditable = this.mapActionToEditable();
    console.log(this.actionEditable);
  }

  public async updateAction() {
    try {
      let action = await this.registeredActionsService.apiRegisteredActionsRegisteredActionPut(this.actionEditable);
      if (action != null) {
        this.action = action;
        this.actionEditable = this.mapActionToEditable();

        this.toaster.showSuccessToast('Successfully saved')
      }
    } catch {
      this.toaster.showErrorToast('Opration failed. Try it again');
    }
  }

  public async closeModal() {
    await this.modalController.dismiss();
  }

  public startAction() {
    this.actionEditable.actionStartedDateTime = new Date();

    this.updateAction();
  }

  public stopAction() {
    this.actionEditable.actionFinishedDateTime = new Date();

    this.updateAction();
  }

  public async addPicture() {
    let image = await this.imageHelper.getImageAsBase64FromDevice();

    if (image != null) {
      this.actionEditable.photo.pictureUri = image;
    }
  }

  private mapActionToEditable() {
    let editable: RegisteredActionDTO = {
      id: this.action.id,
      action: this.action.action,
      actionFinishedDateTime: this.action.actionFinishedDateTime,
      actionStartedDateTime: this.action.actionStartedDateTime,
      clientActionSpecificDescription: this.action.clientActionSpecificDescription,
      comment: this.action.comment,
      estimatedDurationMinutes: this.action.estimatedDurationMinutes,
      isCanceled: this.action.isCanceled,
      isCompleted: this.action.isCompleted,
      photo: this.action.photo,
      plannedStartDateTime: this.action.plannedStartDateTime,
      day: this.action.day
    };

    return editable;
  }
}
