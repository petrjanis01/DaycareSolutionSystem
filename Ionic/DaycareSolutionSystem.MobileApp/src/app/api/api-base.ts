import { HttpHeaders } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';

export class ApiBase {
    private iso8601: RegExp = /^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(\.\d+)?(([+-]\d\d:\d\d)|Z)?$/;
    // TODO load from config
    protected basePath = 'http://localhost:57316';
    
    protected mapDates(object) {
        if (object === null || object === undefined) {
            return object;
        }

        if (typeof object === 'string') {
            if (this.isIso8601(object)) {
                object = new Date(object);
                return object;
            }
        }

        if (typeof object !== 'object') {
            return object;
        }

        for (const key of Object.keys(object)) {
            const value = object[key];

            if (this.isIso8601(value)) {
                object[key] = new Date(value);
            } else if (typeof value === 'object') {
                this.mapDates(value);
            }
        }
        return object;
    }

    private isIso8601(value) {
        if (value === null || value === undefined) {
            return false;
        }

        return this.iso8601.test(value);
    }

    protected checkUserAndCreateAuthHeaders(headers: HttpHeaders): HttpHeaders {
        if (this.isUserLoggedIn() === false) {
            return;
        }

        let token = this.getAuthToken();
        let authHeaderValue = 'Bearer ' + token;

        headers = headers.set('Authorization', authHeaderValue);

        return headers;
    }

    private isUserLoggedIn(): boolean {
        let token = this.getAuthToken();
        if (token != null) {
            return true;
        }
        return false;
    }

    private getAuthToken(): string {
        let token = localStorage.getItem('userAuthToken');
        return token;
    }

}
