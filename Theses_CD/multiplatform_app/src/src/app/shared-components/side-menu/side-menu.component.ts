import { Component } from '@angular/core';
import { VisualHelperService } from 'src/app/services/visual-helper.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
})
export class SideMenuComponent {

  constructor(public visualHelper: VisualHelperService) { }

  navigateToOtherTab(tab: string, e: Event) {
    e.stopPropagation();
    e.preventDefault();
    this.visualHelper.tabs.select(tab);
  }

}
