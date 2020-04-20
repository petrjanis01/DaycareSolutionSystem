import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IndividualPlanDTO, ActionService, IndividualPlansService, AgreedActionService, DayOfWeek } from 'src/app/api/generated';
import { CalendarEvent } from 'angular-calendar';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AgreedActionModalComponent } from './agreed-action-modal/agreed-action-modal.component';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { VisualHelperService } from 'src/app/services/visual-helper.service';

@Component({
  selector: 'app-client-agreed-actions-detail',
  templateUrl: './client-agreed-actions-detail.component.html',
  styleUrls: ['./client-agreed-actions-detail.component.scss']
})
export class ClientAgreedActionsDetailComponent implements OnInit {
  @Input() planId: string;
  @Output() onclose = new EventEmitter();
  public externalEvents: CalendarEvent[];
  public events: CalendarEvent[];

  public viewDate = new Date();

  constructor(private actionService: ActionService, private helper: GeneralHelperService,
    private modal: NgbModal, private datePipe: DatePipe,
    private plansService: IndividualPlansService, private agreedActionsService: AgreedActionService,
    private spinner: NgxSpinnerService, private visualHelper: VisualHelperService) { }

  async ngOnInit() {
    this.createExternalEvents();
    this.reloadAgreedActions();
  }

  private async reloadAgreedActions() {
    let plan = await this.plansService.apiIndividualPlansSinglePlanGet(this.planId);
    this.createEvents(plan);
  }

  private async createExternalEvents() {
    let actions = await this.actionService.apiActionGet();

    let externalEvents = new Array<CalendarEvent>();
    for (let action of actions) {
      let event: CalendarEvent = {
        id: action.id,
        title: action.name,
        start: new Date(),
        draggable: true
      };

      externalEvents.push(event);
    }

    this.externalEvents = externalEvents;
  }

  private createEvents(plan: IndividualPlanDTO) {
    let events = new Array<CalendarEvent>();
    let actionsForDay = plan.actionsForDay;

    for (let actionForDay of actionsForDay) {
      let date = this.helper.getClosestDateThatsDay(actionForDay.day.valueOf());

      let agreedActions = actionForDay.agreedActions;
      for (let agreedAction of agreedActions) {
        let startDate = new Date(date);
        startDate.setHours(agreedAction.plannedStartTime.getHours());
        startDate.setMinutes(agreedAction.plannedStartTime.getMinutes());
        startDate.setSeconds(agreedAction.plannedStartTime.getSeconds());

        let endDate = new Date(date);
        endDate.setHours(agreedAction.plannedEndTime.getHours());
        endDate.setMinutes(agreedAction.plannedEndTime.getMinutes());
        endDate.setSeconds(agreedAction.plannedEndTime.getSeconds());

        let event: CalendarEvent = {
          id: agreedAction.id,
          start: startDate,
          title: agreedAction.action.name,
          draggable: true,
          end: endDate,
        }

        events.push(event);
      }
    }

    this.events = events;
  }

  public eventDropped({ event, newStart, newEnd, allDay }) {
    let externalEvent = this.externalEvents.find(e => e.id === event.id);

    if (externalEvent != null) {
      let modal = this.modal.open(AgreedActionModalComponent,
        { windowClass: `${this.visualHelper.isDarkModeEnabled ? 'dark-modal' : ''}` });
      modal.componentInstance.start = newStart;
      modal.componentInstance.planId = this.planId;
      modal.componentInstance.actionId = externalEvent.id;
      modal.componentInstance.onclose.subscribe((ev) => {
        this.reloadAgreedActions();
      });
    } else {
      let eventAction = this.events.find(e => e.id === event.id);
      let oldStart = this.getOldEventStartTime(event.id);
      if (confirm(`Are you sure you want to reschedule ${eventAction.title} from ${this.datePipe.transform(oldStart, 'EEEE hh:mm aa')}` +
        ` to ${this.datePipe.transform(newStart, 'EEEE hh:mm aa')}?`)) {
        this.moveAgreedAction(eventAction.id as string, newStart, newEnd);
      }
    }
  }

  private async moveAgreedAction(id: string, newStart: Date, newEnd: Date) {
    this.spinner.show();
    let actionToMove = await this.agreedActionsService.apiAgreedActionSingleActionGet(id);

    actionToMove.plannedStartTime = newStart;
    actionToMove.plannedEndTime = newEnd;
    let day = newStart.getDay();
    actionToMove.day = day as DayOfWeek;

    await this.agreedActionsService.apiAgreedActionPut(actionToMove);
    await this.reloadAgreedActions();
    this.spinner.hide();
  }

  private getOldEventStartTime(id: string): Date {
    let event = this.events.find(e => e.id === id);
    let date = new Date(event.start);

    return date;
  }

  public handleEvent(action: string, event: CalendarEvent) {
    if (action === 'clicked') {
      let modal = this.modal.open(AgreedActionModalComponent,
        { windowClass: `${this.visualHelper.isDarkModeEnabled ? 'dark-modal' : ''}` });
      modal.componentInstance.agreedActionId = event.id;
      modal.componentInstance.onclose.subscribe((ev) => {
        this.reloadAgreedActions();
      });
    }
  }

  public close() {
    this.onclose.emit();
  }
}
