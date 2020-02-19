import { NgModule } from '@angular/core';
import { ClientCardComponent } from './client-card/client-card.component';
import { IonicModule } from '@ionic/angular';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { RouterModule } from '@angular/router';


@NgModule({
    declarations: [
        ClientCardComponent,
        SideMenuComponent
    ],
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        RouterModule
    ],
    exports: [
        ClientCardComponent,
        SideMenuComponent
    ]
})
export class SharedComponentsModule { }
