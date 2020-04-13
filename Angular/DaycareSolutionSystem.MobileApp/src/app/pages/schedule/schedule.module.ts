import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { SchedulePage } from './schedule.page';
import { ClientActionsOverviewComponent } from './client-actions-overview/client-actions-overview.component';
import { SharedComponentsModule } from 'src/app/shared-components/share-components.module';
import { ClientActionDetailComponent } from './client-action-detail/client-action-detail.component';

const routes: Routes = [
  {
    path: '',
    component: SchedulePage
  },
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SharedComponentsModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    SchedulePage,
    ClientActionsOverviewComponent,
    ClientActionDetailComponent
  ],
  entryComponents: [ClientActionDetailComponent]
})
export class SchedulePageModule { }
