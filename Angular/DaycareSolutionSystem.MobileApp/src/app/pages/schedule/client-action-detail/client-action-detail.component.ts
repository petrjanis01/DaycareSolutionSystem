import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { RegisteredActionDTO, RegisteredActionsService } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-client-action-detail',
  templateUrl: './client-action-detail.component.html',
  styleUrls: ['./client-action-detail.component.scss'],
})
export class ClientActionDetailComponent implements OnInit {

  @Input() action: RegisteredActionDTO;

  public actionEditable: RegisteredActionDTO;
  public time = '00:00.000';
  public isTimerVisible: boolean;
  public started = null;

  constructor(
    private modalController: ModalController,
    private registeredActionsService: RegisteredActionsService,
    private toaster: ToastService,
    private imageHelper: ImageHelperService,
    public generalHelper: GeneralHelperService
  ) { }

  ngOnInit() {
    this.actionEditable = this.mapActionToEditable();
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
    this.startTimer();
    this.updateAction();
  }

  public stopAction() {
    this.actionEditable.actionFinishedDateTime = new Date();
    this.isTimerVisible = false;
    clearInterval(this.started);
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

    if (editable.isCompleted === false && editable.isCanceled === false && editable.actionStartedDateTime != null) {
      this.startTimer();
    }

    return editable;
  }

  public cancledChanged(ev: any) {
    if (ev && ev.detail && ev.detail.checked && !this.actionEditable.comment) {
      this.toaster.showInfoToast('Please enter reason why is action cancled to comment section.');
    }
  }

  // timer
  public startTimer() {
    this.started = setInterval(this.clockRunning.bind(this), 10);
    this.isTimerVisible = true;
  }

  private clockRunning() {
    let timeBegan: any = this.actionEditable.actionStartedDateTime;
    let currentTime: any = new Date();
    let timeElapsed: any = new Date(currentTime - timeBegan);
    let hour = timeElapsed.getUTCHours();
    let min = timeElapsed.getUTCMinutes();
    let sec = timeElapsed.getUTCSeconds();
    let ms = timeElapsed.getUTCMilliseconds();
    this.time =
      this.zeroPrefix(hour, 2) + ':' +
      this.zeroPrefix(min, 2) + ':' +
      this.zeroPrefix(sec, 2) + '.' +
      this.zeroPrefix(ms, 3);
  }

  private zeroPrefix(num, digit) {
    let zero = '';
    for (let i = 0; i < digit; i++) {
      zero += '0';
    }
    return (zero + num).slice(-digit);
  }

}
