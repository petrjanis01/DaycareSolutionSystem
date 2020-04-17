import { Component, OnInit } from '@angular/core';
import { ActionDTO, ActionService } from 'src/app/api/generated';
import { VisualHelperService } from 'src/app/services/visual-helper.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotifiactionService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.scss']
})
export class ActionsComponent implements OnInit {
  public actions: ActionDTO[];
  public isEditOpen = false;

  public actionForm: FormGroup;

  constructor(private actionService: ActionService, private formBuilder: FormBuilder,
    private spinner: NgxSpinnerService,
    private notifications: NotifiactionService) { }

  ngOnInit() {
    this.realoadActions();
  }

  private async realoadActions() {
    let dtos = await this.actionService.apiActionGet();
    console.log(dtos);
    this.actions = dtos;
  }

  public openActionEdit(id: string) {
    let action = this.actions.find(a => a.id === id);
    this.createActionForm(action);

    this.isEditOpen = true;
  }

  private createActionForm(action?: ActionDTO) {
    this.actionForm = this.formBuilder.group({
      id: new FormControl(action ? action.id : null),
      name: new FormControl(action ? action.name : null, Validators.required),
      generalDescription: new FormControl(action ? action.generalDescription : null, Validators.required)
    })
  }

  public cancelEdit() {
    this.isEditOpen = false;
  }

  public async saveAction() {
    let formValue = this.actionForm.value;
    let isEdit = formValue.id != null;

    let createUpdateDto: ActionDTO = {
      id: formValue.id,
      name: formValue.name,
      generalDescription: formValue.generalDescription
    };

    this.spinner.show();
    try {
      if (isEdit) {
        await this.actionService.apiActionPut(createUpdateDto);
      } else {
        await this.actionService.apiActionPost(createUpdateDto);
      }

      this.notifications.showSuccessNotification('Operation sucessful');
      this.realoadActions();
      this.isEditOpen = false;
    } finally {
      this.spinner.hide();
    }
  }

  public addAction() {
    this.createActionForm();
    this.isEditOpen = true;
  }

  public async deleteAction(id: string) {
    let action = this.actions.find(a => a.id === id);
    if (confirm(`Are you sure you want to delete action: ${action.name}`)) {
      this.spinner.show();
      try {
        await this.actionService.apiActionDelete(id);
        this.notifications.showSuccessNotification('Action deleted');
        this.realoadActions();
        this.isEditOpen = false;
      } finally {
        this.spinner.hide();
      }
    }
  }

}
