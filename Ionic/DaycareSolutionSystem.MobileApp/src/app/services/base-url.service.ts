import { Injectable } from '@angular/core';
import { AppConfig } from './../config/app.config';

@Injectable({ providedIn: 'root' })
export class BaseUrlService {
    public useUrlFromConfig = true;

    public setBaseUrl(url: string) {
        localStorage.setItem('baseUrl', url);
    }

    public getBaseUrl(): string {
        let baseUrl = localStorage.getItem('baseUrl');

        if (this.useUrlFromConfig || baseUrl == null) {
            return AppConfig.settings.apiBaseUrl.baseUrl;
        }

        return baseUrl;
    }
}
