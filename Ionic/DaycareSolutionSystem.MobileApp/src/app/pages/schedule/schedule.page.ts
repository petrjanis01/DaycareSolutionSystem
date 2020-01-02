import { Component, OnInit } from '@angular/core';
import { RegisteredActionsService, RegisteredActionsForDayDTO } from 'src/app/api/generated';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {
  public registeredActions: RegisteredActionsForDayDTO[];

  constructor(private registeredActionsService: RegisteredActionsService, private clientsCache: ClientsCacheService) { }

  async ngOnInit() {
    await this.clientsCache.loaded;

    let dtos = await this.registeredActionsService.apiRegisteredActionsGetRegisteredActionsDetailsGet(10, null);
    this.registeredActions = dtos;

    console.log(this.registeredActions);
  }

  public isToday(date: Date): boolean {
    let today = new Date();

    return date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear();
  }
}
