import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { LoginDTO, AuthService } from '../api/generated';
import { NotifiactionService } from './notification.service';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    constructor(private auth: AuthService, private notification: NotifiactionService, private router: Router) { }

    public async logIn(dto: LoginDTO): Promise<boolean> {
        let result = this.auth.apiAuthLoginPost(dto)
            .then((res) => {
                this.setToken(res.token);

                return true;
            }).catch((err: HttpErrorResponse) => {
                console.error(err);

                this.notification.showErrorNotification('Login failed. Check your credentials and try it again.');
                return false;
            });

        return result;
    }

    public async logOut() {
        localStorage.removeItem('token');
        this.router.navigate(['login']);
    }

    public isLoggedIn(): boolean {
        let token = this.getToken();
        if (token != null) {
            return true;
        }
        return false;
    }

    public getToken(): string {
        let token = localStorage.getItem('token');
        return token;
    }

    private setToken(token: string): void {
        localStorage.setItem('token', token);
    }
}
