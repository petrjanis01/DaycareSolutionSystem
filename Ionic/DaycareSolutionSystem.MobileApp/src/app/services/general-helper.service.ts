import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GeneralHelperService {
    private weekday: Array<string> = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

    public getDayNameByIndex(index: number): string {
        return this.weekday[index];
    }
}
