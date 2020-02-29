import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { MapPage } from './map.page';
import { AgmCoreModule } from '@agm/core';
import { AgmOverlays } from 'agm-overlays';
import { SharedComponentsModule } from 'src/app/shared-components/share-components.module';

const routes: Routes = [
  {
    path: '',
    component: MapPage
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
    }),
    AgmOverlays
  ],
  declarations: [MapPage]
})
export class MapPageModule { }
