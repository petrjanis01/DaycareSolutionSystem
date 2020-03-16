import { HttpHeaders } from '@angular/common/http';
import { AppConfig } from './../config/app.config';
import { NotifiactionService } from '../services/notification.service';
import { Router } from '@angular/router';

export class ApiBase {

    constructor(
        private router: Router,
        private notification: NotifiactionService) { }

    protected get basePath(): string {
        return AppConfig.settings.apiBaseUrl.baseUrl;
    }

    protected processErrors(response: Promise<any>) {
        let processed = response
            .catch(e => {
                let url: string = e.url;

                if (url.endsWith('Auth/login') === false) {
                    if (e.status === 401) {
                        this.notification.showErrorNotification('Token has expired. Please login again.');

                        this.logOut();
                    } else {
                        this.notification.showErrorNotification('Api service is unavailable. Check your internet connection.');
                    }
                }
            });

        return processed;
    }

    protected createAuthHeaders(headers: HttpHeaders): HttpHeaders {
        if (this.isLoggedIn()) {
            let token = this.getToken();
            let authHeaderValue = 'Bearer ' + token;

            headers = headers.set('Authorization', authHeaderValue);

            return headers;
        }
    }

    private isLoggedIn(): boolean {
        let token = this.getToken();
        if (token != null) {
            return true;
        }
        return false;
    }

    private getToken(): string {
        let token = localStorage.getItem('token');
        return token;
    }

    public async logOut() {
        localStorage.removeItem('token');
        this.router.navigate(['login']);
    }
}
