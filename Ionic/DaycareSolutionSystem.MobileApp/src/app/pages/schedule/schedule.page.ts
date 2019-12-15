import { Component, OnInit } from '@angular/core';
import { RegisteredActionsService, RegisteredActionsForDayDTO } from 'src/app/api/generated';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {
  public registeredActions: RegisteredActionsForDayDTO[];

  constructor(private registeredActionsService: RegisteredActionsService) { }

  async ngOnInit() {
    let dtos = await this.registeredActionsService.apiRegisteredActionsGetRegisteredActionsDetailsGet(10, null);
    this.registeredActions = dtos;

    console.log(this.registeredActions);
  }

}
