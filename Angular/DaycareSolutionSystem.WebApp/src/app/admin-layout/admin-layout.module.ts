import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../pages/dashboard/dashboard.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ScheduleComponent } from 'src/app/pages/schedule/schedule.component';

import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { ClientsComponent } from '../pages/clients/clients.component';
import { ProfileComponent } from '../pages/profile/profile.component';
import { ClientDetailComponent } from '../pages/clients/client-detail/client-detail.component';
import { ClientNamePipe } from '../pipes/client-name.pipe';

import { AgmCoreModule } from '@agm/core';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { ClientGeneralInfoComponent } from '../pages/clients/client-detail/client-general-info/client-general-info.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule,
    CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory }),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyCQsd5nCdDeiSvHmgbcSt8Fbk7AOLqPmZw'
    }),
    AngularMultiSelectModule
  ],
  declarations: [
    DashboardComponent,
    ScheduleComponent,
    ClientsComponent,
    ProfileComponent,
    ClientDetailComponent,
    ClientNamePipe,
    ClientGeneralInfoComponent
  ]
})
export class AdminLayoutModule { }
