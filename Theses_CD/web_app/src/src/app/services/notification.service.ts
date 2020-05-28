import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class NotifiactionService {
    private positionClass = 'toast-bottom-right';

    constructor(private toastr: ToastrService) { }

    public showSuccessNotification(message: string) {
        this.toastr.success(`<span class="tim-icons icon-check-2" [data-notify]="icon"></span> ${message}`, '', {
            disableTimeOut: false,
            closeButton: true,
            enableHtml: true,
            timeOut: 1000,
            toastClass: 'alert alert-success alert-with-icon',
            positionClass: 'toast-bottom-right'
        });
    }

    public showInfoNotification(message: string) {
        this.toastr.info(`<span class="tim-icons icon-bell-55" [data-notify]="icon"></span> ${message}`, '', {
            disableTimeOut: false,
            closeButton: true,
            enableHtml: true,
            timeOut: 2000,
            toastClass: 'alert alert-info alert-with-icon',
            positionClass: 'toast-bottom-right'
        });
    }

    public showErrorNotification(message: string) {
        this.toastr.error(`<span class="tim-icons icon-alert-circle-exc" [data-notify]="icon"></span> ${message}`, '', {
            disableTimeOut: false,
            closeButton: true,
            enableHtml: true,
            timeOut: 3000,
            toastClass: 'alert alert-danger alert-with-icon',
            positionClass: 'toast-bottom-right'
        });
    }
}