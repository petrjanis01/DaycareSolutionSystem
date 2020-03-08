import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';

// https://dev.to/imben1109/date-handling-in-angular-application-part-2-angular-http-client-and-ngx-datepicker-3fna
@Injectable()
export class JsonDateInterceptor implements HttpInterceptor {
    private isoDateFormat = /^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(\.\d+)?(([+-]\d\d:\d\d)|Z)?$/;
    private tzoffset = (new Date()).getTimezoneOffset() * 60000;

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let clone = req.clone();
        this.convertToIso(clone.body);

        return next.handle(clone).pipe(map((val: HttpEvent<any>) => {
            if (val instanceof HttpResponse || val instanceof HttpRequest) {
                const body = val.body;
                this.convertFromIso(body);
            }
            return val;
        }));
    }

    private convertToIso(body: any) {
        if (body === null || body === undefined) {
            return body;
        }
        if (typeof body !== 'object') {
            return body;
        }
        for (const key of Object.keys(body)) {
            const value = body[key];
            if (value instanceof Date) {
                let date: any = value;
                body[key] = (new Date(date - this.tzoffset)).toISOString().slice(0, -1);
            } else if (typeof value === 'object') {
                this.convertToIso(value);
            }
        }
    }

    private isIsoDateString(value: any): boolean {
        if (value === null || value === undefined) {
            return false;
        }
        if (typeof value === 'string') {
            return this.isoDateFormat.test(value);
        }
        return false;
    }

    private convertFromIso(body: any) {
        if (body === null || body === undefined) {
            return body;
        }
        if (typeof body !== 'object') {
            return body;
        }
        for (const key of Object.keys(body)) {
            const value = body[key];
            if (this.isIsoDateString(value)) {
                body[key] = new Date(value);
            } else if (typeof value === 'object') {
                this.convertFromIso(value);
            }
        }
    }
}
