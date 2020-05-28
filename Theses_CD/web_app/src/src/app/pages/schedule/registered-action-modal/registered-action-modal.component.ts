import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {
  RegisteredActionDTO, RegisteredActionsService, EmployeeService, ActionService,
  ActionDTO, EmployeeBasicDTO, ClientsService, ClientBasicsDTO
} from 'src/app/api/generated';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { DatepickerDateModel } from 'src/app/shared/datepicker-date-model';
import { TimepickerTimeModel } from 'src/app/shared/timepicker-time-model';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-registered-action-modal',
  templateUrl: './registered-action-modal.component.html',
  styleUrls: ['./registered-action-modal.component.scss']
})
export class RegisteredActionModalComponent implements OnInit {
  @Input() registeredAction: RegisteredActionDTO;
  @Output() onclose = new EventEmitter();

  public actions: ActionDTO[];
  public employees: EmployeeBasicDTO[];
  public clients: ClientBasicsDTO[];

  public editForm: FormGroup;

  constructor(private registeredActionsService: RegisteredActionsService, private employeeService: EmployeeService,
    private actionsService: ActionService, private modal: NgbModal,
    private formBuilder: FormBuilder, private clientsService: ClientsService,
    private helper: GeneralHelperService, private spinner: NgxSpinnerService) { }

  async ngOnInit() {
    this.actions = await this.actionsService.apiActionGet();
    this.employees = await this.employeeService.apiEmployeeAllCaregiversGet();
    this.clients = await this.clientsService.apiClientsAllClientBasicsGet();
    this.createEditForm();
  }

  public closeModal() {
    this.onclose.emit();
    this.modal.dismissAll();
  }

  private createEditForm() {
    if (this.registeredAction) {
      let plannedStartTime = new Date();
      plannedStartTime.setHours(this.registeredAction.plannedStartDateTime.getHours());
      plannedStartTime.setMinutes(this.registeredAction.plannedStartDateTime.getMinutes());
      plannedStartTime.setSeconds(this.registeredAction.plannedStartDateTime.getSeconds());

      let plannedStartDate = new Date();
      plannedStartDate.setFullYear(this.registeredAction.plannedStartDateTime.getFullYear());
      plannedStartDate.setMonth(this.registeredAction.plannedStartDateTime.getMonth());
      plannedStartDate.setDate(this.registeredAction.plannedStartDateTime.getDate());

      this.editForm = this.formBuilder.group({
        action: new FormControl(this.registeredAction.action.id, Validators.required),
        employee: new FormControl(this.registeredAction.employeeId, Validators.required),
        plannedStartTime: new FormControl(new TimepickerTimeModel(plannedStartTime), Validators.required),
        plannedStartDate: new FormControl(new DatepickerDateModel(plannedStartDate), Validators.required),
        isCanceled: new FormControl(this.registeredAction.isCanceled),
        comment: new FormControl(this.registeredAction.comment)
      });

      return;
    }

    this.editForm = this.formBuilder.group({
      action: new FormControl(this.actions[0].id, Validators.required),
      employee: new FormControl(this.employees[0].id, Validators.required),
      plannedStartTime: new FormControl(new TimepickerTimeModel(new Date()), Validators.required),
      plannedStartDate: new FormControl(new DatepickerDateModel(new Date()), Validators.required),
      comment: new FormControl(''),
      client: new FormControl(this.clients[0].id)
    });
  }

  public isValid(): boolean {
    let isValid = this.editForm.valid;
    if (this.registeredAction && this.editForm.value.isCanceled) {
      isValid = isValid && (this.editForm.value.comment);
    }

    return isValid;
  }

  public async saveAction() {
    this.spinner.show();
    if (this.registeredAction) {
      this.updateDtoFromForm();
      await this.registeredActionsService.apiRegisteredActionsRegisteredActionPersistentPut(this.registeredAction);
    } else {
      let dto = this.createDtoFromForm();
      await this.registeredActionsService.apiRegisteredActionsPost(dto);
    }

    this.spinner.hide();
    this.closeModal();
  }

  private updateDtoFromForm() {
    let formValue = this.editForm.value;

    this.registeredAction.actionId = formValue.action;
    this.registeredAction.employeeId = formValue.employee;
    this.registeredAction.plannedStartDateTime = this.getPlannedStartDateTime();
    this.registeredAction.isCanceled = formValue.isCanceled;
    this.registeredAction.comment = formValue.comment;
  }

  private createDtoFromForm(): RegisteredActionDTO {
    let formValue = this.editForm.value;
    let dto: RegisteredActionDTO = {};

    dto.actionId = formValue.action;
    dto.employeeId = formValue.employee;
    dto.plannedStartDateTime = this.getPlannedStartDateTime();
    dto.comment = formValue.comment;
    dto.clientId = formValue.client;

    return dto;
  }

  private getPlannedStartDateTime(): Date {
    let timeModel: TimepickerTimeModel = this.editForm.value.plannedStartTime;
    let dateModel: DatepickerDateModel = this.editForm.value.plannedStartDate;
    let plannedTime = this.helper.getDateFromTimepickerModel(timeModel);
    let plannedDate = this.helper.getDateFromDatepickerModel(dateModel);

    let date = new Date(plannedDate);
    date.setHours(plannedTime.getHours());
    date.setMinutes(plannedTime.getMinutes());
    date.setSeconds(plannedTime.getSeconds());

    return date;
  }
}
