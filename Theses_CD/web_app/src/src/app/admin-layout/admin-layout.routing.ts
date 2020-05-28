import { Routes } from '@angular/router';

import { ScheduleComponent } from 'src/app/pages/schedule/schedule.component';
import { ProfileComponent } from '../pages/profile/profile.component';
import { ClientsComponent } from '../pages/clients/clients.component';
import { ClientDetailComponent } from '../pages/clients/client-detail/client-detail.component';
import { EmployeesComponent } from '../pages/employees/employees.component';
import { ActionsComponent } from '../pages/actions/actions.component';
import { EmployeeDetailComponent } from '../pages/employees/employee-detail/employee-detail.component';

export const AdminLayoutRoutes: Routes = [
  { path: 'schedule', component: ScheduleComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'clients', component: ClientsComponent },
  { path: 'actions', component: ActionsComponent },
  { path: 'employees', component: EmployeesComponent },
  { path: 'clients/detail/:id', component: ClientDetailComponent },
  { path: 'employees/detail/:id', component: EmployeeDetailComponent },
];
