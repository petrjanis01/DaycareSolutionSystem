import { Injectable } from '@angular/core';
import { IonTabs, Platform } from '@ionic/angular';
import { RegisteredActionDTO, AgreedActionDTO } from '../api/generated';

@Injectable({ providedIn: 'root' })
export class VisualHelperService {
    public isSideMenuVisibe: boolean;
    public tabs: IonTabs;

    constructor(private platform: Platform) { }

    public getBgCssColorForRegisteredAction(action: RegisteredActionDTO): string {
        if (action.isCanceled) {
            return 'bg-canceled';
        } else if (action.isCompleted) {
            return 'bg-completed';
        } else if (action.isCompleted === false && action.actionStartedDateTime != null) {
            return 'bg-in-progress';
        }

        return 'bg-default';
    }

    public getBasicDataGridSize() {
        return this.isSideMenuVisibe ? 6 : 12;
    }

    public getMapDetailAvatarGridSize() {
        return this.isSideMenuVisibe ? 1 : 2;
    }

    public getMapDetailNameGridSize() {
        return this.isSideMenuVisibe ? 9 : 8;

    }

    public getClientCardGridSize() {
        return this.isSideMenuVisibe ? 3 : 12;
    }
}
