import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Injectable({ providedIn: 'root' })
export class ToastService {
    constructor(private toastController: ToastController) { }

    async showSuccessToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 2000,
            color: 'success'
        });
        toast.present();
    }

    async showErrorToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 3000,
            color: 'danger'
        });
        toast.present();
    }
}
