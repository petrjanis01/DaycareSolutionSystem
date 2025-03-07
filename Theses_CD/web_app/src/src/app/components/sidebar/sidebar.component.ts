import { Component, OnInit } from '@angular/core';

declare interface RouteInfo {
  path: string;
  title: string;
  icon: string;
  class: string;
}
export const ROUTES: RouteInfo[] = [
  {
    path: '/schedule',
    title: 'Schedule',
    icon: 'icon-calendar-60',
    class: ''
  },
  {
    path: '/clients',
    title: 'Clients',
    icon: 'icon-single-02',
    class: ''
  },
  {
    path: '/employees',
    title: 'Eployees',
    icon: 'icon-badge',
    class: ''
  },
  {
    path: '/actions',
    title: 'Actions',
    icon: 'icon-user-run',
    class: ''
  },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];

  constructor() { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
  }

  isMobileMenu() {
    if (window.innerWidth > 991) {
      return false;
    }
    return true;
  }
}
