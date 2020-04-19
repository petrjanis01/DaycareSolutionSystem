import { Component, OnInit } from '@angular/core';
import { VisualHelperService } from '../services/visual-helper.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {
  public sidebarColor = 'red';

  constructor(private visualHelper: VisualHelperService) { }

  changeSidebarColor(color) {
    this.visualHelper.themeColor = color;
    let sidebar = document.getElementsByClassName('sidebar')[0];
    let mainPanel = document.getElementsByClassName('main-panel')[0];

    this.sidebarColor = color;

    if (sidebar !== undefined) {
      sidebar.setAttribute('data', color);
    }
    if (mainPanel !== undefined) {
      mainPanel.setAttribute('data', color);
    }
  }
  changeDashboardColor(color) {
    let body = document.getElementsByTagName('body')[0];
    if (body && color === 'white-content') {
      this.visualHelper.isDarkModeEnabled = false;
      body.classList.add(color);
    }
    else if (body.classList.contains('white-content')) {
      body.classList.remove('white-content');
      this.visualHelper.isDarkModeEnabled = true;
    }
  }
  ngOnInit() { }
}
