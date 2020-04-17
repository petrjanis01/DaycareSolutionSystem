import { Component, OnInit, Input } from '@angular/core';
import { IndividualPlanDTO, IndividualPlansService, IndividualPlanCreateUpdateDTO } from 'src/app/api/generated';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { DatepickerDateModel } from '../datepicker-date-model';
import { SliderAnimation } from 'src/app/components/slider-animation';
import { trigger, transition } from '@angular/animations';
import { NotifiactionService } from 'src/app/services/notification.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-client-individual-plans',
  templateUrl: './client-individual-plans.component.html',
  styleUrls: ['./client-individual-plans.component.scss'],
  animations: [
    trigger('editOpenSlider', [
      transition(':increment', SliderAnimation.right),
      transition(':decrement', SliderAnimation.left),
    ]),
  ]
})
export class ClientIndividualPlansComponent implements OnInit {
  @Input() clientId: string;
  public plans: IndividualPlanDTO[];
  public editOpenCounter = 0;
  public planEditForm: FormGroup
  public isFromUntilWarningVisible = false;

  constructor(private plansService: IndividualPlansService, private formBuilder: FormBuilder,
    private spinner: NgxSpinnerService, private notifications: NotifiactionService,
    private helper: GeneralHelperService) { }

  ngOnInit() {
    this.reloadPlans();
  }

  private async reloadPlans() {
    let dtos = await this.plansService.apiIndividualPlansAgreedActionsByPlansGet(this.clientId);
    console.log(dtos);
    this.plans = dtos;
  }

  public addPlan() {
    this.createEditPlanForm();
    this.editOpenCounter++;
  }

  public openPlanEdit(id: string) {
    let plan = this.plans.find(p => p.id === id);
    this.createEditPlanForm(plan);

    this.editOpenCounter++;
  }

  private createEditPlanForm(plan?: IndividualPlanDTO) {
    console.log(plan);
    let dateUntil = new Date();
    dateUntil.setFullYear(dateUntil.getFullYear() + 1);
    dateUntil = plan ? plan.validUntil : dateUntil;

    let dateFrom = plan ? plan.validFrom : new Date();

    this.planEditForm = this.formBuilder.group({
      id: new FormControl(plan ? plan.id : null),
      validFrom: new FormControl(new DatepickerDateModel(dateFrom), Validators.required),
      validUntil: new FormControl(new DatepickerDateModel(dateUntil), Validators.required)
    });
  }

  public async deletePlan(id: string) {
    this.spinner.show();
    try {
      await this.plansService.apiIndividualPlansDelete(id);
      this.notifications.showSuccessNotification('Operation sucessful');
      this.reloadPlans();
    } finally {
      this.spinner.hide();
      this.editOpenCounter = 0;
    }
  }

  public openPlanDetail(id: string) {

  }

  public cancelEdit() {
    this.editOpenCounter--;
  }

  public async savePlan() {
    let formValue = this.planEditForm.value;

    let fromModel: DatepickerDateModel = formValue.validFrom;
    let untilModel: DatepickerDateModel = formValue.validUntil;

    let dateFrom = this.helper.getDateFromDatepickerModel(fromModel);
    let dateUntil = this.helper.getDateFromDatepickerModel(untilModel);

    if (dateUntil > dateFrom) {
      this.isFromUntilWarningVisible = false;
    } else {
      this.isFromUntilWarningVisible = true;
      return;
    }

    let dto: IndividualPlanCreateUpdateDTO = {
      id: formValue.id,
      clientId: this.clientId,
      validFrom: dateFrom,
      validUntil: dateUntil
    }

    this.spinner.show();
    try {
      if (dto.id != null) {
        await this.plansService.apiIndividualPlansPut(dto);
      } else {
        await this.plansService.apiIndividualPlansPost(dto);
      }

      this.reloadPlans();
      this.notifications.showSuccessNotification('Operation sucessful');
      this.editOpenCounter--;
    } finally {
      this.spinner.hide();
    }
  }
}
