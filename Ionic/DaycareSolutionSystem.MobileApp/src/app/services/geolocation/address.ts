import { AddressDTO } from 'src/app/api/generated';

export class Address {
    postCode: string;
    city: string;
    street?: string;
    buildingNumber: string;
    gpsCoordinates: string;

    constructor(dto?: AddressDTO) {
        if (dto != null) {
            this.gpsCoordinates = dto.gpsCoordinates;
            this.city = dto.city;
            this.buildingNumber = dto.buildingNumber;
            this.postCode = dto.postCode;
            this.street = dto.street;
        }
    }

    public toString(): string {
        let result = '';

        result += this.street != null ? `${this.street} ${this.buildingNumber}` : this.buildingNumber;
        result += `, ${this.city}, ${this.postCode}`;

        return result;
    }
}
