import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { ActionService } from './api/action.service';
import { AuthService } from './api/auth.service';
import { ClientsService } from './api/clients.service';
import { EmployeeService } from './api/employee.service';
import { IndividualPlansService } from './api/individualPlans.service';
import { RegisteredActionsService } from './api/registeredActions.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    ActionService,
    AuthService,
    ClientsService,
    EmployeeService,
    IndividualPlansService,
    RegisteredActionsService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
