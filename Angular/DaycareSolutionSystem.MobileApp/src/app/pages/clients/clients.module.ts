import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { ClientsPage } from './clients.page';
import { SharedComponentsModule } from 'src/app/shared-components/share-components.module';
import { ClientDetailPage } from './client-detail/client-detail.page';
import { ClientDetailPageModule } from './client-detail/client-detail.module';

const routes: Routes = [
  {
    path: '',
    component: ClientsPage
  },
  {
    path: 'client-detail/:id',
    component: ClientDetailPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    ClientDetailPageModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes),
    SharedComponentsModule
  ],
  declarations: [ClientsPage]
})
export class ClientsPageModule { }
