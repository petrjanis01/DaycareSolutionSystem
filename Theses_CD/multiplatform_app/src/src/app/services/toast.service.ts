import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Injectable({ providedIn: 'root' })
export class ToastService {
    constructor(private toastController: ToastController) { }

    public async showSuccessToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 1000,
            color: 'success'
        });
        toast.present();
    }

    public async showErrorToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 3000,
            color: 'danger'
        });
        toast.present();
    }

    public async showInfoToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 2500,
            color: 'secondary'
        });
        toast.present();
    }
}
