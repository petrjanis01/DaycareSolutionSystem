import { Injectable } from '@angular/core';
import { IonTabs } from '@ionic/angular';
import { RegisteredActionDTO, AgreedActionDTO } from '../api/generated';

@Injectable({ providedIn: 'root' })
export class VisualHelperService {
    public isSideMenuVisibe: boolean;
    public tabs: IonTabs;

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

    // public getBgCssColorForAgreedAction(action: AgreedActionDTO): string {

    //     return css;
    // }
}
