import { Injectable } from '@angular/core';
import { AppConfig } from './../config/app.config';

@Injectable({ providedIn: 'root' })
export class BaseUrlService {

    get useUrlFromConfig(): boolean {
        let useUrlFromConfig = localStorage.getItem('useUrlFromConfig');

        if (useUrlFromConfig != null) {
            return useUrlFromConfig === 'true';
        }

        return true;
    }

    set useUrlFromConfig(useUrlFromConfig: boolean) {
        localStorage.setItem('useUrlFromConfig', useUrlFromConfig.toString());
    }

    public setBaseUrl(url: string) {
        localStorage.setItem('baseUrl', url);
    }

    public getBaseUrl(): string {
        let baseUrl = localStorage.getItem('baseUrl');

        if (this.useUrlFromConfig || baseUrl == null) {
            return AppConfig.settings.apiBaseUrl;
        }
        return baseUrl;
    }
}
