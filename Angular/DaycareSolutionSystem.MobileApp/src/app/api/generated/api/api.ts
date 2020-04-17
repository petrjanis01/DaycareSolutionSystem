export * from './action.service';
import { ActionService } from './action.service';
export * from './auth.service';
import { AuthService } from './auth.service';
export * from './clients.service';
import { ClientsService } from './clients.service';
export * from './employee.service';
import { EmployeeService } from './employee.service';
export * from './individualPlans.service';
import { IndividualPlansService } from './individualPlans.service';
export * from './registeredActions.service';
import { RegisteredActionsService } from './registeredActions.service';
export const APIS = [ActionService, AuthService, ClientsService, EmployeeService, IndividualPlansService, RegisteredActionsService];
