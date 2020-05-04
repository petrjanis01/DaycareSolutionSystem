import { Injectable, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {
    constructor(@Inject(APP_BASE_HREF) private baseHref: string) { }

    public weekdays: Array<string> = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    private gender: Array<string> = ['Male', 'Female'];

    public getDayNameByIndex(index: number): string {
        return this.weekdays[index];
    }

    public getGenderString(index: number): string {
        return this.gender[index - 1];
    }

    public compareDatesWithoutTime(date1: Date, date2: Date): boolean {
        let dateWithoutTime1 = new Date(date1).setHours(0, 0, 0, 0);
        let dateWithoutTime2 = new Date(date2).setHours(0, 0, 0, 0);

        return dateWithoutTime1 === dateWithoutTime2;
    }

    public getAnonymousImgUrlFormatted(): string {
        let imgUrl = `${this.baseHref != null ? this.baseHref : ''}/assets/img/user-anonymous.png`;

        return imgUrl;
    }
}
