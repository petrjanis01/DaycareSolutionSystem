export class DatepickerDateModel {
    public year: number;
    public month: number;
    public day: number;

    constructor(date: Date) {
        this.year = date.getFullYear();
        this.month = date.getMonth();
        this.day = date.getDate();
    }
}