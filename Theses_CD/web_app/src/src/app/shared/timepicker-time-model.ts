import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

export class TimepickerTimeModel implements NgbTimeStruct {
    public hour: number;
    public minute: number;
    public second: number;

    constructor(date: Date) {
        this.hour = date.getHours();
        this.minute = date.getMinutes();
        this.second = date.getSeconds();
    }
}