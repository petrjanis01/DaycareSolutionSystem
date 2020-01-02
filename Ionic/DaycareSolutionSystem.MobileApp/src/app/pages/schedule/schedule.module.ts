import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { SchedulePage } from './schedule.page';
import { ClientActionsOverviewComponent } from './client-actions-overview/client-actions-overview.component';
import { ClientActionDetailPage } from './client-action-detail/client-action-detail.page';

const routes: Routes = [
  {
    path: '',
    component: SchedulePage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    SchedulePage,
    ClientActionsOverviewComponent,
    ClientActionDetailPage
  ],
  entryComponents: [ClientActionDetailPage]
})
export class SchedulePageModule { }
