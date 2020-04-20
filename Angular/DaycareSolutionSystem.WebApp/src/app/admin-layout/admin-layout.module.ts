import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdminLayoutRoutes } from './admin-layout.routing';

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
import { NgxSpinnerModule } from 'ngx-spinner';
import { EmployeesComponent } from '../pages/employees/employees.component';
import { ActionsComponent } from '../pages/actions/actions.component';
import { ClientIndividualPlansComponent } from '../pages/clients/client-detail/client-individual-plans/client-individual-plans.component';
import { ClientAgreedActionsDetailComponent } from '../pages/clients/client-detail/client-individual-plans/client-agreed-actions-detail/client-agreed-actions-detail.component';
import { AgreedActionModalComponent } from '../pages/clients/client-detail/client-individual-plans/client-agreed-actions-detail/agreed-action-modal/agreed-action-modal.component';
import { EmployeeNamePipe } from '../pipes/employee-name.pipe';
import { EmployeeDetailComponent } from '../pages/employees/employee-detail/employee-detail.component';
import { RegisteredActionModalComponent } from '../pages/schedule/registered-action-modal/registered-action-modal.component';

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
    AngularMultiSelectModule,
    NgxSpinnerModule,
  ],
  declarations: [
    ScheduleComponent,
    ClientsComponent,
    ProfileComponent,
    ClientDetailComponent,
    ClientNamePipe,
    EmployeeNamePipe,
    ClientGeneralInfoComponent,
    EmployeesComponent,
    ActionsComponent,
    ClientIndividualPlansComponent,
    ClientAgreedActionsDetailComponent,
    AgreedActionModalComponent,
    EmployeeDetailComponent,
    RegisteredActionModalComponent
  ],
  providers: [
    DatePipe
  ],
  entryComponents: [
    AgreedActionModalComponent,
    RegisteredActionModalComponent
  ]
})
export class AdminLayoutModule { }
