import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenticationService } from './authentication.service';

// https://medium.com/@ryanchenkie_40935/angular-authentication-using-route-guards-bf7a4ca13ae3
@Injectable({ providedIn: 'root' })
export class AuthGuardService implements CanActivate {

    constructor(public auth: AuthenticationService, public router: Router) { }

    canActivate(): boolean {
        if (!this.auth.isLoggedIn()) {
            this.router.navigate(['login']);
            return false;
        }
        return true;
    }
}
