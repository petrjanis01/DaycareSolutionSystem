import { Component, OnInit } from '@angular/core';
import { RegisteredActionsService, RegisteredActionDTO, ClientDTO, ClientBasicsDTO, EmployeeBasicDTO, ClientsService, EmployeeService } from 'src/app/api/generated';
import { CalendarEvent } from 'angular-calendar';
import { DatePipe } from '@angular/common';
import { NotifiactionService } from 'src/app/services/notification.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RegisteredActionModalComponent } from './registered-action-modal/registered-action-modal.component';
import { VisualHelperService } from 'src/app/services/visual-helper.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

const colors: any = {
  canceled: {
    primary: '#ff8080',
  },
  completed: {
    primary: '#80ff80',
  },
  inProgress: {
    primary: '#ffff80',
  },
  default: {
    primary: '#4ddbff'
  }
};

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  private registeredActions: RegisteredActionDTO[];
  public clients: ClientBasicsDTO[];
  public employees: EmployeeBasicDTO[];
  public clientsPersist: ClientBasicsDTO[];

  public events: CalendarEvent[];

  public selectedClients = new Array<ClientBasicsDTO>();
  public selectedEmployee: EmployeeBasicDTO;
  public selectedEmployeeId = null;

  public curretlyViewedDate = new Date();
  public activeDayIsOpen = false;

  constructor(private registeredActionsService: RegisteredActionsService, private clientsService: ClientsService,
    private employeesService: EmployeeService, private datePipe: DatePipe,
    private notifications: NotifiactionService, private spinner: NgxSpinnerService,
    private modal: NgbModal, private visualHelper: VisualHelperService,
    private generalHelper: GeneralHelperService) { }

  async ngOnInit() {
    await this.loadClients();
    this.loadEmployees();
    this.reloadRegisteredActions(this.curretlyViewedDate);
  }

  private async loadClients() {
    let clients = await this.clientsService.apiClientsAllClientBasicsGet();
    this.clientsPersist = new Array<ClientBasicsDTO>();
    this.clientsPersist = this.clientsPersist.concat(clients);

    this.clients = clients;

    let pleaseSelectClient: ClientBasicsDTO = {};
    pleaseSelectClient.fullName = 'Please select client';
    pleaseSelectClient.id = null;
    this.clients.unshift(pleaseSelectClient);
  }

  private async loadEmployees() {
    this.employees = await this.employeesService.apiEmployeeAllCaregiversGet();
    this.addEmptyEmployee();
  }

  private addEmptyEmployee() {
    let pleaseSelectEmployee: EmployeeBasicDTO = {};
    pleaseSelectEmployee.fullName = 'Please select employee';
    pleaseSelectEmployee.id = null;
    this.employees.unshift(pleaseSelectEmployee);
  }

  private async reloadRegisteredActions(date: Date) {
    let actions = await this.registeredActionsService.apiRegisteredActionsAllActionsMonthGet(date);
    this.registeredActions = actions;

    this.reloadEvents(actions);
  }

  public getMonth(): string {
    let month = this.curretlyViewedDate.getMonth();
    let montName = this.generalHelper.months[month];
    return montName;
  }

  private reloadEvents(actions: RegisteredActionDTO[]) {
    let events = new Array<CalendarEvent>();

    for (let action of actions) {
      let startDate = new Date(action.plannedStartDateTime);
      let endDate = new Date(action.plannedStartDateTime);
      endDate.setMinutes(endDate.getMinutes() + action.estimatedDurationMinutes);
      let client = this.clientsPersist.find(c => c.id === action.clientId);
      let eventTitle = `${action.action.name} for ${client.fullName} from ${this.datePipe.transform(startDate, 'short')} to ${this.datePipe.transform(endDate, 'short')}`;

      let event: CalendarEvent = {
        id: action.id,
        start: startDate,
        title: eventTitle,
        draggable: true,
        end: endDate,
        color: this.getActionColor(action)
      }

      events.push(event);
    }

    this.events = events;
  }

  private getActionColor(action: RegisteredActionDTO) {
    if (action.isCompleted) {
      return colors.completed;
    }

    if (action.isCanceled) {
      return colors.canceled;
    }

    if (action.actionStartedDateTime != null) {
      return colors.inProgress;
    }

    return colors.default;
  }

  public navigateCalendarLeft() {
    let date = new Date(this.curretlyViewedDate);
    date.setMonth(date.getMonth() - 1);
    this.curretlyViewedDate = date;
    this.reloadRegisteredActions(this.curretlyViewedDate);
    this.activeDayIsOpen = false;
  }

  public navigateCalendarRight() {
    let date = new Date(this.curretlyViewedDate);
    date.setMonth(date.getMonth() + 1);
    this.curretlyViewedDate = date;
    this.reloadRegisteredActions(this.curretlyViewedDate);
    this.activeDayIsOpen = false;
  }

  public onClientSelectChange(id: string) {
    if (id == null) {
      return;
    }

    let client = this.clients.find(c => c.id === id);
    let index = this.clients.indexOf(client);
    this.clients.splice(index, 1);

    this.selectedClients.push(client);
    this.clients.sort((a, b) => this.compareClients(a, b));
    this.filterActions();
  }

  private compareClients(a: ClientDTO, b: ClientDTO): number {
    if (a.id == null) {
      return -1;
    }

    if (b.id == null) {
      return 1;
    }

    return a.fullName.localeCompare(b.fullName);
  }

  public removeClient(id: string) {
    let client = this.selectedClients.find(c => c.id === id);
    let index = this.selectedClients.indexOf(client);
    this.selectedClients.splice(index, 1);

    this.clients.push(client);
    this.clients.sort((a, b) => this.compareClients(a, b));
    this.filterActions();
  }

  public onEmployeeSelectChange(id: string) {
    if (id == null) {
      return;
    }
    let employee = this.employees.find(c => c.id === id);
    this.selectedEmployee = employee;
    this.filterActions();
  }

  public removeEmployee(id: string) {
    this.selectedEmployee = null;
    this.filterActions();
  }

  private filterActions() {
    let filteredActions = new Array<RegisteredActionDTO>();

    if (this.selectedClients.length === 0 && this.selectedEmployee == null) {
      this.reloadEvents(this.registeredActions);
      return;
    }

    if (this.selectedClients.length === 0 && this.selectedEmployee != null) {
      let actions = this.registeredActions.filter(ra => ra.employeeId === this.selectedEmployee.id);
      this.reloadEvents(actions);
      return;
    }

    for (let client of this.selectedClients) {
      let clientFilteredActions = this.registeredActions.filter(ra => ra.clientId === client.id && (this.selectedEmployee != null
        ? ra.employeeId === this.selectedEmployee.id : true));
      for (let action of clientFilteredActions) {
        if (!filteredActions.find(a => a.id === action.id)) {
          filteredActions.push(action);
        }
      }
    }

    this.reloadEvents(filteredActions);
  }

  public handleEvent(action: string, event: CalendarEvent) {
    if (action === 'Clicked') {
      let registeredAction = this.registeredActions.find(rc => rc.id === event.id);

      let modal = this.modal.open(RegisteredActionModalComponent,
        { windowClass: `${this.visualHelper.isDarkModeEnabled ? 'dark-modal' : ''}` });
      modal.componentInstance.registeredAction = registeredAction;
      modal.componentInstance.onclose.subscribe((ev) => {
        this.reloadRegisteredActions(this.curretlyViewedDate);
      });
    }
  }

  public eventDropped({ event, newStart, newEnd, allDay }) {
    let movedAction = this.registeredActions.find(rc => rc.id === event.id);

    if (movedAction.isCanceled || movedAction.isCompleted || movedAction.actionStartedDateTime != null) {
      this.notifications.showInfoNotification('Cannot reschedule actions that are cancled, completed or in progress');
      return;
    }

    let client = this.clientsPersist.find(c => c.id === movedAction.clientId);

    if (confirm(`Are you sure you want to reschedule ${movedAction.action.name} for ${client.fullName} from ${this.datePipe.transform(movedAction.plannedStartDateTime, 'short')}` +
      ` to ${this.datePipe.transform(newStart, 'short')}?`)) {
      this.moveAction(movedAction, newStart);
    }
  }

  private async moveAction(movedAction: RegisteredActionDTO, newStart) {
    this.spinner.show();

    movedAction.plannedStartDateTime = newStart;
    let newAction = await this.registeredActionsService.apiRegisteredActionsRegisteredActionPersistentPut(movedAction);

    let action = this.registeredActions.find(rc => rc.id === newAction.id);
    let index = this.registeredActions.indexOf(action);
    this.registeredActions.splice(index, 1);
    this.registeredActions.push(newAction);
    this.filterActions();

    this.spinner.hide();
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (date.getMonth() === this.curretlyViewedDate.getMonth()) {
      this.activeDayIsOpen = !((this.curretlyViewedDate.getDate() === date.getDate() && this.activeDayIsOpen === true)
        || events.length === 0);

      this.curretlyViewedDate = date;
    }
  }

  public addNewAction() {
    let modal = this.modal.open(RegisteredActionModalComponent,
      { windowClass: `${this.visualHelper.isDarkModeEnabled ? 'dark-modal' : ''}` });
    modal.componentInstance.onclose.subscribe((ev) => {
      this.reloadRegisteredActions(this.curretlyViewedDate);
    });
  }

  public async generateActions() {
    this.spinner.show();
    await this.registeredActionsService.apiRegisteredActionsGenerateActionsForPeriodPost();
    await this.reloadRegisteredActions(this.curretlyViewedDate);
    this.spinner.hide();
  }
}
