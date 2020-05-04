import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../config/app.config';
import { map } from 'rxjs/operators';
import { CoordinatesDTO, AddressDTO } from 'src/app/api/generated';
import { NotifiactionService } from './notification.service';

@Injectable({ providedIn: 'root' })
export class GeolocationHelperService {
    private googleMapApiUrlBase = 'https://maps.googleapis.com/maps/api';
    private apiKey = AppConfig.settings.googleMapsApiKey;

    constructor(private http: HttpClient, private notifications: NotifiactionService) { }

    public async getCurrentLocation(): Promise<CoordinatesDTO> {
        // tslint:disable-next-line:no-shadowed-variable
        let cords = await new Promise<CoordinatesDTO>((resolve, reject) => {
            navigator.geolocation.getCurrentPosition(resp => {
                resolve({ latitude: resp.coords.latitude, longitude: resp.coords.longitude });
            },
                err => {
                    reject(err);
                });
        }).catch(err => {
            this.notifications.showInfoNotification('Unable to get device location.');
        });

        if (cords != null) {
            return cords as CoordinatesDTO;
        }
    }

    public async  getFullAddressIfNeeded(address: AddressDTO) {
        await this.getAddressIfNeeded(address);
        await this.getCoordinatesIfNeeded(address);
    }

    private async getAddressIfNeeded(address: AddressDTO) {
        let isAddressIncomplete = address.city == null || address.buildingNumber == null || address.postCode == null;
        if (isAddressIncomplete) {
            let addressCalculated = await (await this.getAddressFromgGpsCoordinates(address.coordinates));
            address.buildingNumber = addressCalculated.buildingNumber;
            address.street = addressCalculated.street;
            address.postCode = addressCalculated.postCode;
            address.city = addressCalculated.city;
        }
    }

    private async getCoordinatesIfNeeded(address: AddressDTO) {
        if (address.coordinates == null) {
            let coordinates = await (await this.getGpsCoordinatesFromAddress(address));
            address.coordinates = coordinates;
        }
    }

    public async getGpsCoordinatesFromAddress(address: AddressDTO): Promise<CoordinatesDTO> {
        let result = await this.http.get<any>(`${this.googleMapApiUrlBase}/geocode/json?address=${this.adrressToApiConsumableString(address)}&key=${this.apiKey}`)
            .pipe(
                map(geoData => {
                    if (!geoData || !geoData.results || geoData.results === 0) {
                        return null;
                    }
                    return geoData.results[0].geometry.location;
                })
            ).toPromise();

        if (result != null) {
            let coordinates: CoordinatesDTO = {
                latitude: +result.lat.toString(),
                longitude: +result.lng.toString()
            };
            return coordinates;
        }
    }

    private adrressToApiConsumableString(address: AddressDTO): string {
        let result = '';

        result += address.street != null ? `${address.street} ${address.buildingNumber}` : address.buildingNumber;
        result += `, ${address.city}, ${address.postCode}`;

        return result;
    }

    public async getAddressFromgGpsCoordinates(coordinates: CoordinatesDTO): Promise<AddressDTO> {
        let addressComponents = await
            this.http.get<any>(`${this.googleMapApiUrlBase}/geocode/json?latlng=${coordinates.latitude},${coordinates.longitude}
            &key=${this.apiKey}`)
                .pipe(
                    map(geoData => {
                        if (!geoData || !geoData.results || geoData.results === 0) {
                            return null;
                        }
                        return geoData.results[0].address_components;
                    })
                ).toPromise();

        if (addressComponents == null) {
            return null;
        }

        let address = this.mapAddressComponentsToAdress(addressComponents);
        address.coordinates = coordinates;

        return address;
    }

    private mapAddressComponentsToAdress(
        addressComponents: Array<{ long_name: string, short_name: string, types: Array<string> }>
    ): AddressDTO {

        let premise = addressComponents.find(a => !!a.types.find(t => t === 'premise'));
        let streetNumber = addressComponents.find(a => !!a.types.find(t => t === 'street_number')).long_name;
        let buildingNumberResult = premise != null ? `${premise.long_name}/${streetNumber}` : streetNumber;

        let postCodeResult = addressComponents.find(a => !!a.types.find(t => t === 'postal_code')).long_name;
        let cityResult = addressComponents.find(a => !!a.types.find(t => t === 'locality')).long_name;
        let streetResult = addressComponents.find(a => !!a.types.find(t => t === 'route'));

        let address: AddressDTO = {
            postCode: postCodeResult,
            buildingNumber: buildingNumberResult,
            city: cityResult,
            street: (streetResult != null ? streetResult.long_name : null)
        };
        return address;
    }
}
