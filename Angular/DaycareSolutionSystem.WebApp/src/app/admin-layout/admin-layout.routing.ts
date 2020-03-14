import { Routes } from '@angular/router';

import { DashboardComponent } from '../pages/dashboard/dashboard.component';
import { ScheduleComponent } from 'src/app/pages/schedule/schedule.component';

export const AdminLayoutRoutes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'schedule', component: ScheduleComponent },
];
