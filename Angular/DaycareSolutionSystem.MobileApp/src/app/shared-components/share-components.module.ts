import { NgModule } from '@angular/core';
import { ClientCardComponent } from './client-card/client-card.component';
import { IonicModule } from '@ionic/angular';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { RouterModule } from '@angular/router';
import { ActionItemCardComponent } from './action-item-card/action-item-card.component';


@NgModule({
    declarations: [
        ClientCardComponent,
        SideMenuComponent,
        ActionItemCardComponent
    ],
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        RouterModule
    ],
    exports: [
        ClientCardComponent,
        SideMenuComponent,
        ActionItemCardComponent
    ]
})
export class SharedComponentsModule { }
