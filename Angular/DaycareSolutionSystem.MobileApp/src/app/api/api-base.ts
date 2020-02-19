import { HttpHeaders } from '@angular/common/http';
import { BaseUrlService } from '../services/base-url.service';
import { NavController } from '@ionic/angular';
import { ToastService } from '../services/toast.service';

export class ApiBase {

    constructor(
        private baseUrlService: BaseUrlService,
        private nav: NavController,
        private toast: ToastService) { }

    protected get basePath(): string {
        return this.baseUrlService.getBaseUrl();
    }

    protected processErrors(response: Promise<any>) {
        let processed = response
            .catch(e => {
                if (e.status === 401) {
                    this.toast.showErrorToast('Token has expired. Please login again.');

                    this.logOut();
                } else {
                    this.toast.showErrorToast(e.Detail.Message);
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
        this.nav.navigateRoot('/login');
    }
}
