import { Injectable, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {

    constructor(@Inject(APP_BASE_HREF) private baseHref: string) { }

    private weekdays: Array<string> = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
    public genders: Array<string> = ['Male', 'Female'];

    public getDayNameByIndex(index: number): string {
        return this.weekdays[index];
    }

    public getGenderString(index: number): string {
        return this.genders[index - 1];
    }

    public getAnonymousImgUrlFormatted(): string {
        let imgUrl = `${this.baseHref != null ? this.baseHref : ''}/assets/img/user-anonymous.png`;

        return imgUrl;
    }
}