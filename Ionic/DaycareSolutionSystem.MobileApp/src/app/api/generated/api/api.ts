export * from './auth.service';
import { AuthService } from './auth.service';
export * from './employee.service';
import { EmployeeService } from './employee.service';
export * from './registeredActions.service';
import { RegisteredActionsService } from './registeredActions.service';
export * from './test.service';
import { TestService } from './test.service';
export const APIS = [AuthService, EmployeeService, RegisteredActionsService, TestService];
