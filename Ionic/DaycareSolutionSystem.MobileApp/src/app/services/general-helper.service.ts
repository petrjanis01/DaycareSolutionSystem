import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

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
}
