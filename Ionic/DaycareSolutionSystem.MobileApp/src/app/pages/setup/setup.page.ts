import { Component, OnInit } from '@angular/core';
import { AppConfig } from './../../config/app.config';
import { BaseUrlService } from 'src/app/services/base-url.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-setup',
  templateUrl: './setup.page.html',
  styleUrls: ['./setup.page.scss'],
})
export class SetupPage {
  public baseUrl = AppConfig.settings.apiBaseUrl.baseUrl;
  public useUrlFromConfig = true;

  constructor(private baseUrlService: BaseUrlService, private toasterService: ToastService) { }

  public save() {
    this.baseUrlService.useUrlFromConfig = this.useUrlFromConfig;
    this.baseUrlService.setBaseUrl(this.baseUrl);
    this.toasterService.showSuccessToast(`Non congi api url changed to: ${this.baseUrl}`);
  }
}
