import { Pipe, PipeTransform } from '@angular/core';
import { ClientBasicsDTO } from 'src/app/api/generated';

@Pipe({
    name: 'clientName'
})
export class ClientNamePipe implements PipeTransform {
    transform(allClients: ClientBasicsDTO[], name: string): ClientBasicsDTO[] {
       name = name ?? '';
        return allClients.filter(client => client.fullName.toLowerCase().includes(name.toLowerCase()));
    }

}