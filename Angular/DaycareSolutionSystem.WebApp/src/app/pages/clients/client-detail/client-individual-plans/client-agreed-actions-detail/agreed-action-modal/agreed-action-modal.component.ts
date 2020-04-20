import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AgreedActionService, ActionService, AgreedActionDTO, ActionDTO, EmployeeService, EmployeeBasicDTO } from 'src/app/api/generated';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotifiactionService } from 'src/app/services/notification.service';
import { TimepickerTimeModel } from 'src/app/shared/timepicker-time-model';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-agreed-action-modal',
  templateUrl: './agreed-action-modal.component.html',
  styleUrls: ['./agreed-action-modal.component.scss']
})
export class AgreedActionModalComponent implements OnInit {
  @Input() agreedActionId: string;
  @Input() actionId: string;
  @Input() start: Date;
  @Input() planId: string;
  @Output() onclose = new EventEmitter<any>();

  private agreedAction: AgreedActionDTO;
  public editForm: FormGroup;
  public actions: ActionDTO[];
  public employees: EmployeeBasicDTO[];

  constructor(private modal: NgbModal, private agrredActionService: AgreedActionService,
    private actionService: ActionService, private formBulder: FormBuilder,
    private employeeService: EmployeeService, private spinner: NgxSpinnerService,
    private notifications: NotifiactionService, public helper: GeneralHelperService) { }

  async ngOnInit() {
    this.actions = await this.actionService.apiActionGet();
    this.employees = await this.employeeService.apiEmployeeAllCaregiversGet();
    this.createEditForm();
  }

  private async createEditForm() {
    let action: AgreedActionDTO;
    if (this.agreedActionId) {
      action = await this.agrredActionService.apiAgreedActionSingleActionGet(this.agreedActionId);
      this.agreedAction = action;
    }

    let day;
    if (this.start) {
      day = this.start.getDay();
    }

    let fromModel = new TimepickerTimeModel(action ? action.plannedStartTime : this.start);
    let untilTime = action ? new Date() : new Date(this.start);
    untilTime.setMinutes(untilTime.getMinutes() + 30);

    let untilModel = new TimepickerTimeModel(action ? action.plannedEndTime : untilTime);

    this.editForm = this.formBulder.group({
      action: new FormControl(action ? action.action.id : this.actionId, Validators.required),
      employee: new FormControl(action ? action.employeeId : this.employees[0].id, Validators.required),
      day: new FormControl(action ? action.day : day, Validators.required),
      fromTime: new FormControl(fromModel, Validators.required),
      untilTime: new FormControl(untilModel, Validators.required),
      description: new FormControl(action ? action.clientActionSpecificDescription : '', Validators.required)
    });
  }

  public async deleteAction() {
    if (confirm(`Are you sure you want to delete ${this.agreedAction.action.name} on ${this.helper.getDayNameByIndex(this.agreedAction.day)}?`)) {
      this.spinner.show();
      await this.agrredActionService.apiAgreedActionDelete(this.agreedActionId);
      this.spinner.hide();
      this.close();
    }
  }

  public async saveAction() {
    this.spinner.show();
    if (this.agreedAction) {
      this.mapFormValuesToDto(this.agreedAction);
      await this.agrredActionService.apiAgreedActionPut(this.agreedAction);
    } else {
      let dto: AgreedActionDTO = {};
      this.mapFormValuesToDto(dto);
      dto.individualPlanId = this.planId;
      await this.agrredActionService.apiAgreedActionPost(dto);
    }

    this.spinner.hide();
    this.close();
  }

  private mapFormValuesToDto(dto: AgreedActionDTO) {
    let formValue = this.editForm.value;
    dto.action = {};
    dto.action.id = formValue.action;
    dto.employeeId = formValue.employee;
    dto.day = formValue.day;
    dto.clientActionSpecificDescription = formValue.description;
    dto.plannedStartTime = this.helper.getDateFromTimepickerModel(formValue.fromTime);
    dto.plannedEndTime = this.helper.getDateFromTimepickerModel(formValue.untilTime);
    dto.estimatedDurationMinutes = null;
  }

  close() {
    this.onclose.emit();
    this.modal.dismissAll();
  }

}
