import { Component, Input } from '@angular/core';
import { AgreedActionsForDayDTO } from 'src/app/api/generated/model/agreedActionsForDayDTO';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'agreed-actions-overview',
  templateUrl: './agreed-actions-overview.component.html',
  styleUrls: ['./agreed-actions-overview.component.scss'],
})
export class AgreedActionsOverviewComponent {
  @Input() actionsForDay: AgreedActionsForDayDTO;

  constructor(public generalHelper: GeneralHelperService) { }

}
