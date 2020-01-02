import { Gender, PictureDTO, AddressDTO } from 'src/app/api/generated';

export class Client {
    public id: string;
    public firstName: string;
    public surname: string;
    public fullName: string;
    public birthDate: Date;
    public gender: Gender;
    public profilePicture: string;
    public address: AddressDTO;
    public distanceFromDevice: number;
}