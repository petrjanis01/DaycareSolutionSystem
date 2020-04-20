import { Component, OnInit, ViewChild } from '@angular/core';
import { RegisteredActionsService, RegisteredActionsForDayDTO, RegisteredActionDTO } from 'src/app/api/generated';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { IonDatetime } from '@ionic/angular';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { VisualHelperService } from 'src/app/services/visual-helper.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {
  @ViewChild(IonDatetime) datePicker: IonDatetime;
  public registeredActions: RegisteredActionsForDayDTO[];
  public selectedDate: string;
  public minDate: Date;
  public maxDate: Date;
  public ionDateTimeConsumableMinDate: string;
  public ionDateTimeConsumableMaxDate: string;
  public isPreviousBtnEnabled = true;
  public isNextBtnEnabled = true;


  constructor(
    private registeredActionsService: RegisteredActionsService,
    private clientsCache: ClientsCacheService,
    private generalHelper: GeneralHelperService,
    public visualHelper: VisualHelperService
  ) {
    this.selectedDate = new Date().toISOString();
  }

  ngOnInit() {
    this.reloadData();
  }

  public async reloadData() {
    this.registeredActionsService.apiRegisteredActionsLastRegisteredActionsMinMaxDateGet().then(dto => {
      this.minDate = dto.minDate;
      this.maxDate = dto.maxDate;

      this.ionDateTimeConsumableMinDate = this.convertDateToIonDateTimeConsumableString(this.minDate);
      this.ionDateTimeConsumableMaxDate = this.convertDateToIonDateTimeConsumableString(this.maxDate);
    });

    await this.clientsCache.loaded;
    let itemCount = 10;
    if (this.registeredActions && this.registeredActions.length === 0) {
      itemCount = this.registeredActions.length;
    }

    this.registeredActions = null;
    let date = new Date(this.selectedDate);

    let dtos = await this.registeredActionsService.apiRegisteredActionsRegisteredActionsGet(itemCount, date, null);
    this.registeredActions = dtos;
    console.log(dtos);
  }

  public isToday(date: Date): boolean {
    let today = new Date();

    return date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear();
  }

  public loadMoreData(event) {
    let lastDate = this.registeredActions[this.registeredActions.length - 1].date;
    let lastActionsClient = this.registeredActions[this.registeredActions.length - 1].registeredActionsClient;
    let actionsForLastClient = lastActionsClient[lastActionsClient.length - 1].registeredActions;
    let lastDisplayedAction = actionsForLastClient[actionsForLastClient.length - 1];

    let date = new Date(this.selectedDate);
    this.registeredActionsService.apiRegisteredActionsRegisteredActionsGet(10, date, lastDisplayedAction.id)
      .then(dtos => {

        if (this.generalHelper.compareDatesWithoutTime(lastDate, dtos[0].date)
          && lastActionsClient[lastActionsClient.length - 1].clientId === dtos[0].registeredActionsClient[0].clientId) {
          dtos = this.mergeData(dtos);
        }
        this.registeredActions = this.registeredActions.concat(dtos);

        let lastAction = this.registeredActions[this.registeredActions.length - 1];
        event.target.disabled = lastAction.containsLast;

      }).finally(() => event.target.complete());
  }

  private mergeData(dtos: RegisteredActionsForDayDTO[]): RegisteredActionsForDayDTO[] {
    let actionsClientToMerge = dtos[0].registeredActionsClient[0].registeredActions;

    let lastActionsClient = this.registeredActions[this.registeredActions.length - 1].registeredActionsClient;
    let actionsForLastClient = lastActionsClient[lastActionsClient.length - 1].registeredActions;
    this.registeredActions[this.registeredActions.length - 1]
      .registeredActionsClient[(this.registeredActions[this.registeredActions.length - 1].registeredActionsClient.length) - 1]
      .registeredActions
      = actionsForLastClient.concat(actionsClientToMerge);

    dtos[0].registeredActionsClient.splice(0, 1);

    if (dtos[0].registeredActionsClient.length === 0) {
      dtos.splice(0, 1);
    }

    return dtos;
  }

  public previousDayClicked() {
    let date = new Date(this.selectedDate);
    date.setDate(date.getDate() - 1);
    this.selectedDate = date.toISOString();
    this.reloadData();

    let minDateCopy = new Date(this.minDate);
    this.isPreviousBtnEnabled = date.setDate(date.getDate() - 1) < minDateCopy.setHours(0, 0, 0, 0);
  }

  public nextDayClicked() {
    let date = new Date(this.selectedDate);
    date.setDate(date.getDate() + 1);
    this.selectedDate = date.toISOString();
    this.reloadData();

    let maxDateCopy = new Date(this.maxDate);
    this.isNextBtnEnabled = date.setDate(date.getDate() + 1) > maxDateCopy.setHours(0, 0, 0, 0);
  }

  public async openDatePicker() {
    await this.datePicker.open();
  }

  // TODO solve enabling/disabling btns
  public selectedDateChange(event: any) {
    if (event && event.detail && event.detail.value) {
      let date = event.detail.value;
      this.selectedDate = new Date(date).toISOString();
      this.reloadData();

      let minDateCopy = new Date(this.minDate);
      this.isPreviousBtnEnabled = new Date(date).setDate(date.getDate() - 1) < minDateCopy.setHours(0, 0, 0, 0);

      let maxDateCopy = new Date(this.maxDate);
      this.isNextBtnEnabled = new Date(date).setDate(date.getDate() + 1) > maxDateCopy.setHours(0, 0, 0, 0);
    }
  }

  public convertDateToIonDateTimeConsumableString(date: Date) {
    let year = date.getFullYear().toString();
    let month = date.getMonth().toString();
    let day = date.getDate().toString();

    if (date.getMonth() < 10) {
      month = `0${month}`;
    }

    if (date.getDate() < 10) {
      day = `0${day}`;
    }

    return `${year}-${month}-${day}`;
  }
}
