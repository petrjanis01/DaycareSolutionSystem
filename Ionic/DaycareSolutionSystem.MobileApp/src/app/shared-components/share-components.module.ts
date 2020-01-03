import { NgModule } from '@angular/core';
import { ClientCardComponent } from './client-card/client-card.component';
import { IonicModule } from '@ionic/angular';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@NgModule({
    declarations: [
        ClientCardComponent,
    ],
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
    ],
    exports: [
        ClientCardComponent,
    ]
})
export class SharedComponentsModule { }
