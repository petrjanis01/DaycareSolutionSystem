import { Injectable, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { DatepickerDateModel } from '../shared/datepicker-date-model';
import { TimepickerTimeModel } from '../shared/timepicker-time-model';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {

    constructor(@Inject(APP_BASE_HREF) private baseHref: string) { }

    public weekdays: Array<string> = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    public genders: Array<string> = ['Male', 'Female'];
    public positions: Array<string> = ['Caregiver', 'Manager'];
    public months: Array<string> = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

    public getDayNameByIndex(index: number): string {
        return this.weekdays[index];
    }

    public getPositionByIndex(index: number): string {
        return this.positions[index];
    }

    public getGenderString(index: number): string {
        return this.genders[index - 1];
    }

    public getAnonymousImgUrlFormatted(): string {
        let imgUrl = `${this.baseHref != null ? this.baseHref : ''}/assets/img/user-anonymous.png`;

        return imgUrl;
    }

    public getDateFromDatepickerModel(model: DatepickerDateModel): Date {
        let date = new Date();
        date.setDate(model.day);
        date.setFullYear(model.year);
        date.setMonth(model.month - 1);

        return date;
    }

    public getDateFromTimepickerModel(model: TimepickerTimeModel): Date {
        let date = new Date();
        date.setHours(model.hour);
        date.setMinutes(model.minute);
        date.setSeconds(model.second);

        return date;
    }

    public getClosestDateThatsDay(day: number): Date {
        let date = new Date();
        let dayOfDate = date.getDay();
        date.setDate(date.getDate() + (day - dayOfDate));

        return date;
    }
}