import { Injectable } from '@angular/core';
import { IonTabs } from '@ionic/angular';

@Injectable({ providedIn: 'root' })
export class VisualHelperService {
    public isSideMenuVisibe: boolean;
    public tabs: IonTabs;
}
