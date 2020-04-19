import { Component, OnInit, Input } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AgreedActionService, ActionService, AgreedActionDto, ActionDTO, EmployeeService } from 'src/app/api/generated';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-agreed-action-modal',
  templateUrl: './agreed-action-modal.component.html',
  styleUrls: ['./agreed-action-modal.component.scss']
})
export class AgreedActionModalComponent implements OnInit {
  @Input() actionId: string;
  @Input() start: Date;
  @Input() planId: string;

  private agreedAction: AgreedActionDto;
  public editForm: FormGroup;
  public actions: ActionDTO[];

  constructor(private modal: NgbModal, private agrredActionService: AgreedActionService,
    private actionService: ActionService, private formBulder: FormBuilder,
    private employeeService: EmployeeService) { }

  async ngOnInit() {
    this.actions = await this.actionService.apiActionGet();
    this.createEditForm();
  }

  private async createEditForm() {
    let action: AgreedActionDto;
    if (this.actionId) {
      action = await this.agrredActionService.apiAgreedActionSingleActionGet(this.actionId);
    }

    this.editForm = this.formBulder.group({
      action: new FormControl(action ? action.action.id : this.actions[0].id, Validators.required),
      employee: new FormControl(Validators.required),
      day: new FormControl(Validators.required),
      fromTime: new FormControl(Validators.required),
      untilTime: new FormControl(Validators.required),
      description: new FormControl(Validators.required)
    });
  }

  close() {
    this.modal.dismissAll();
  }

}
