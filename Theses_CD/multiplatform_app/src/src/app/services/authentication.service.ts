import { NavController } from '@ionic/angular';
import { Injectable } from '@angular/core';
import { LoginDTO, AuthService } from '../api/generated';
import { ToastService } from './toast.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    constructor(private nav: NavController, private auth: AuthService, private toastService: ToastService) { }

    public async logIn(dto: LoginDTO): Promise<boolean> {
        let result = this.auth.apiAuthLoginPost(dto)
            .then((res) => {
                this.setToken(res.token);

                return true;
            }).catch((err: HttpErrorResponse) => {
                console.error(err);

                this.toastService.showErrorToast('Login failed. Check your credentials and try it again.');
                return false;
            });

        return result;
    }

    public async logOut() {
        localStorage.removeItem('token');
        this.nav.navigateRoot('/login');
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
