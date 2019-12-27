import { HttpHeaders } from '@angular/common/http';
import { BaseUrlService } from '../services/base-url.service';
import { NavController } from '@ionic/angular';
import { ToastService } from '../services/toast.service';

export class ApiBase {
    private iso8601: RegExp = /^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(\.\d+)?(([+-]\d\d:\d\d)|Z)?$/;

    constructor(
        private baseUrlService: BaseUrlService,
        private nav: NavController,
        private toast: ToastService) { }

    protected get basePath(): string {
        return this.baseUrlService.getBaseUrl();
    }

    protected processResponse(response: Promise<any>) {
        let processed = response.then(resp => this.mapDates(resp))
            .catch(e => {
                if (e.status === 401) {
                    this.toast.showErrorToast('Token has expired. Please login again.');

                    this.logOutAndNavigateLoginPage();
                } else {
                    this.toast.showErrorToast(e.Detail.Message);
                }
            });

        return processed;
    }

    private mapDates(object) {
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

    private removeAuthToken(): void {
        localStorage.removeItem('userAuthToken');
    }

    public async logOutAndNavigateLoginPage() {
        await this.removeAuthToken();
        this.nav.navigateRoot('/login');
    }
}
