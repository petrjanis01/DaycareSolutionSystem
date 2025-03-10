import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { ComponentsModule } from './components/components.module';

import { APP_INITIALIZER } from '@angular/core';
import { AppConfig } from './config/app.config';

import { JsonDateInterceptor } from './api/json-date-interceptor';
import { NotifiactionService } from './services/notification.service';
import { AuthGuardService } from './services/auth-guard.service';

import { APP_BASE_HREF, PlatformLocation } from '@angular/common';

export function initializeApp(appConfig: AppConfig) {
  return () => appConfig.load();
}

function trimLastSlashFromUrl(baseUrl: string) {
  if (!baseUrl) {
    return null;
  } else if (baseUrl[baseUrl.length - 1] === '/') {
    let trimmedUrl = baseUrl.substring(0, baseUrl.length - 1);
    return trimmedUrl;
  }
}


@NgModule({
  imports: [
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ComponentsModule,
    NgbModule,
    RouterModule,
    AppRoutingModule,
    ToastrModule.forRoot(),
  ],
  declarations: [AppComponent, AdminLayoutComponent],
  providers: [
    AuthGuardService,
    AppConfig,
    NotifiactionService,
    { provide: HTTP_INTERCEPTORS, useClass: JsonDateInterceptor, multi: true },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfig], multi: true
    },
    {
      provide: APP_BASE_HREF,
      useFactory: (s: PlatformLocation) => trimLastSlashFromUrl(s.getBaseHrefFromDOM()),
      deps: [PlatformLocation]
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
