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
                this.setAuthToken(res.token);

                return true;
            }).catch((err: HttpErrorResponse) => {
                console.error(err);

                this.toastService.showErrorToast(err.message);
                return false;
            });

        return result;
    }

    public async logOutAndNavigateLoginPage() {
        await this.removeAuthToken();
        this.nav.navigateRoot('/login');
    }

    public isUserLoggedIn(): boolean {
        let token = this.getAuthToken();
        if (token != null) {
            return true;
        }
        return false;
    }

    public getAuthToken(): string {
        let token = localStorage.getItem('userAuthToken');
        return token;
    }

    private setAuthToken(token: string): void {
        localStorage.setItem('userAuthToken', token);
    }

    private removeAuthToken(): void {
        localStorage.removeItem('userAuthToken');
    }
}
