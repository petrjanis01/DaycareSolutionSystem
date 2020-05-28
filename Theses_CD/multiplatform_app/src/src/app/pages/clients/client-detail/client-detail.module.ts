import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { ClientDetailPage } from './client-detail.page';
import { SharedComponentsModule } from 'src/app/shared-components/share-components.module';

import { AgmCoreModule } from '@agm/core';

const routes: Routes = [
  {
    path: ':id',
    component: ClientDetailPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SharedComponentsModule,
    RouterModule.forChild(routes),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyCQsd5nCdDeiSvHmgbcSt8Fbk7AOLqPmZw'
    })
  ],
  declarations: [ClientDetailPage]
})
export class ClientDetailPageModule { }
