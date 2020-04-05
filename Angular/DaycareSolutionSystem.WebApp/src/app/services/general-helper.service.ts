import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {
    private weekdays: Array<string> = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
    public genders: Array<string> = ['Male', 'Female'];

    public getDayNameByIndex(index: number): string {
        return this.weekdays[index];
    }

    public getGenderString(index: number): string {
        return this.genders[index - 1];
    }
}