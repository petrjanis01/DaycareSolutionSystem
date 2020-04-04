import { Routes } from '@angular/router';

import { DashboardComponent } from '../pages/dashboard/dashboard.component';
import { ScheduleComponent } from 'src/app/pages/schedule/schedule.component';
import { ProfileComponent } from '../pages/profile/profile.component';
import { ClientsComponent } from '../pages/clients/clients.component';
import { ClientDetailComponent } from '../pages/clients/client-detail/client-detail.component';

export const AdminLayoutRoutes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'schedule', component: ScheduleComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'clients', component: ClientsComponent },
  { path: 'clients/detail/:id', component: ClientDetailComponent },
];
