import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {
    private weekday: Array<string> = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
    private gender: Array<string> = ['Male', 'Female'];

    public getDayNameByIndex(index: number): string {
        return this.weekday[index];
    }

    public getGenderString(index: number): string {
        return this.gender[index - 1];
    }

    public compareDatesWithoutTime(date1: Date, date2: Date): boolean {
        let dateWithoutTime1 = new Date(date1).setHours(0, 0, 0, 0);
        let dateWithoutTime2 = new Date(date2).setHours(0, 0, 0, 0);

        return dateWithoutTime1 === dateWithoutTime2;
    }
}
