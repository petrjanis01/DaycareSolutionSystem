import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../config/app.config';
import { map } from 'rxjs/operators';
import { Address } from './address';
import { Plugins } from '@capacitor/core';

// https://medium.com/@shawinshawz/ionic-4-google-maps-geo-location-b49d7f1d1111
@Injectable({ providedIn: 'root' })
export class GeolocationHelperService {
    private googleMapApiUrlBase = 'https://maps.googleapis.com/maps/api';
    private apiKey = AppConfig.settings.googleMaps.apiKey;

    constructor(private http: HttpClient) { }

    public async getCurrentLocation(): Promise<string> {
        let result = await Plugins.Geolocation.getCurrentPosition();

        return `${result.coords.latitude},${result.coords.longitude}`;
    }

    public async getGpsCoordinatesFromAddress(address: Address): Promise<string> {
        let result = await this.http.get<any>(`${this.googleMapApiUrlBase}/geocode/json?address=${address.toString()}&key=${this.apiKey}`)
            .pipe(
                map(geoData => {
                    if (!geoData || !geoData.results || geoData.results === 0) {
                        return null;
                    }
                    return geoData.results[0].geometry.location;
                })
            ).toPromise();

        if (result != null) {
            return `${result.lat},${result.lng}`;
        }
    }

    public async getAddressFromgGpsCoordinates(gpsCoordinates: string): Promise<Address> {
        let addressComponents = await
            this.http.get<any>(`${this.googleMapApiUrlBase}/geocode/json?latlng=${gpsCoordinates}&key=${this.apiKey}`)
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
        return address;
    }

    private mapAddressComponentsToAdress(
        addressComponents: Array<{ long_name: string, short_name: string, types: Array<string> }>): Address {
        let address = new Address();

        let premise = addressComponents.find(a => a.types.find(t => t === 'premise')).long_name;
        let streetNumber = addressComponents.find(a => a.types.find(t => t === 'street_number'));
        address.buildingNumber = streetNumber != null ? `${premise}/${streetNumber.long_name}` : premise;

        let postCode = addressComponents.find(a => a.types.find(t => t === 'postal_code')).long_name;
        address.postCode = postCode;

        let city = addressComponents.find(a => a.types.find(t => t === 'locality')).long_name;
        address.city = city;

        let street = addressComponents.find(a => a.types.find(t => t === 'route'));
        if (street != null) {
            address.street = street.long_name;
        }

        return address;
    }
}
