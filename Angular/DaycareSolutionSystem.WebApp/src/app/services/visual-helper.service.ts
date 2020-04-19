import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class VisualHelperService {
    public themeColor = 'red';
    public isDarkModeEnabled = true;
}