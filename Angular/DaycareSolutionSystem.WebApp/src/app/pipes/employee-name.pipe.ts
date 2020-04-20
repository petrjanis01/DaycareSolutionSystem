import { Pipe, PipeTransform } from '@angular/core';
import { EmployeeBasicDTO } from 'src/app/api/generated';

@Pipe({
    name: 'employeeName'
})
export class EmployeeNamePipe implements PipeTransform {
    transform(allEmployees: EmployeeBasicDTO[], name: string): EmployeeBasicDTO[] {
        name = name ?? '';
        return allEmployees.filter(employee => employee.fullName.toLowerCase().includes(name.toLowerCase()));
    }

}