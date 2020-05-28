import { Gender, AddressDTO } from 'src/app/api/generated';
import { Address } from '../geolocation/address';

export class Client {
    public id: string;
    public firstName: string;
    public surname: string;
    public fullName: string;
    public birthDate: Date;
    public gender: Gender;
    public profilePicture: string;
    public address: Address;
    public email: string;
    public phoneNumber: string;
    // in meters
    public distanceFromDevice: number;
}
