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

  public isToday(date: Date): boolean {
    let today = new Date();

    return today.getFullYear === date.getFullYear
      && today.getMonth === date.getMonth
      && today.getDate === date.getDate;
  }

  public getDafaultImageIfNotExists(img: string): string {
    if (img == null) {
      return './../../../assets/img/user-anonymous.png';
    }

    return img;
  }

}
