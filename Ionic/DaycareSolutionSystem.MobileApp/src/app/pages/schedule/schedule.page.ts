import { Component, OnInit } from '@angular/core';
import { RegisteredActionsService, RegisteredActionsForDayDTO, RegisteredActionDTO } from 'src/app/api/generated';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {
  public registeredActions: RegisteredActionsForDayDTO[];

  constructor(private registeredActionsService: RegisteredActionsService, private clientsCache: ClientsCacheService) { }

  ngOnInit() {
    this.reloadData();
  }

  public async reloadData() {
    await this.clientsCache.loaded;

    let itemCount = 10;
    if (this.registeredActions && this.registeredActions.length === 0) {
      itemCount = this.registeredActions.length;
    }

    this.registeredActions = null;

    let dtos = await this.registeredActionsService.apiRegisteredActionsRegisteredActionsGet(itemCount, null);
    this.registeredActions = dtos;

    console.log(this.registeredActions);
  }

  public isToday(date: Date): boolean {
    let today = new Date();

    return date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear();
  }

  public loadData(event) {
    let lastDisplayedAction = this.getLastDisplayedAction();

    this.registeredActionsService.apiRegisteredActionsRegisteredActionsGet(10, lastDisplayedAction.id)
      .then(dtos => {
        this.registeredActions = this.registeredActions.concat(dtos);

        let lastAction = this.registeredActions[this.registeredActions.length - 1];
        event.target.disabled = lastAction.containsLast;
        console.log(this.registeredActions);

      }).finally(() => event.target.complete());
  }

  private getLastDisplayedAction(): RegisteredActionDTO {
    let lastActionsClient = this.registeredActions[this.registeredActions.length - 1].registeredActionsClient;
    let actionsForLastClient = lastActionsClient[lastActionsClient.length - 1].registeredActions;
    let lastAction = actionsForLastClient[actionsForLastClient.length - 1];

    return lastAction;
  }
}
