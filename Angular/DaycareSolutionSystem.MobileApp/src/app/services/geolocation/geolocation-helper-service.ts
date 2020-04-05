import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../config/app.config';
import { map } from 'rxjs/operators';
import { Address } from './address';
import { Plugins } from '@capacitor/core';
import { CoordinatesDTO } from 'src/app/api/generated';
import { Platform } from '@ionic/angular';
import { ToastService } from '../toast.service';

// https://medium.com/@shawinshawz/ionic-4-google-maps-geo-location-b49d7f1d1111
@Injectable({ providedIn: 'root' })
export class GeolocationHelperService {
    private googleMapApiUrlBase = 'https://maps.googleapis.com/maps/api';
    private apiKey = AppConfig.settings.googleMaps.apiKey;

    constructor(private http: HttpClient, private platform: Platform, private toast: ToastService) { }

    public async getCurrentLocation(): Promise<CoordinatesDTO> {
        if (this.platform.is('capacitor')) {
            let result = await Plugins.Geolocation.getCurrentPosition();

            let coordinates: CoordinatesDTO = {
                latitude: result.coords.latitude,
                longitude: result.coords.longitude
            };
            return coordinates;
        } else {
            // tslint:disable-next-line:no-shadowed-variable
            let cords = await new Promise<CoordinatesDTO>((resolve, reject) => {
                navigator.geolocation.getCurrentPosition(resp => {
                    resolve({ latitude: resp.coords.latitude, longitude: resp.coords.longitude });
                },
                    err => {
                        reject(err);
                    });
            }).catch(err => {
                this.toast.showErrorToast('Unable to get device location.');
            });

            if (cords != null) {
                return cords as CoordinatesDTO;
            }
        }
    }

    public async getGpsCoordinatesFromAddress(address: Address): Promise<CoordinatesDTO> {
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
            let coordinates: CoordinatesDTO = {
                latitude: result.lat.toString(),
                longitude: result.lng.toString()
            };
            return coordinates;
        }
    }

    public async getAddressFromgGpsCoordinates(coordinates: CoordinatesDTO): Promise<Address> {
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

    // https://stackoverflow.com/questions/1502590/calculate-distance-between-two-points-in-google-maps-v3
    public getDistnaceBetween2Coordinates(cord1: CoordinatesDTO, cord2: CoordinatesDTO): number {
        // Earth’s mean radius in meter
        const R = 6378137;

        let dLat = this.getRads(+cord2.latitude - +cord1.latitude);
        let dLong = this.getRads(+cord2.longitude - +cord1.longitude);

        let a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(this.getRads(+cord1.latitude)) * Math.cos(this.getRads(+cord2.latitude)) *
            Math.sin(dLong / 2) * Math.sin(dLong / 2);
        let c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        let d = R * c;

        return d;
    }

    private getRads(x: number): number {
        return x * Math.PI / 180;
    }

    private mapAddressComponentsToAdress(
        addressComponents: Array<{ long_name: string, short_name: string, types: Array<string> }>
    ): Address {
        let address = new Address();

        let premise = addressComponents.find(a => !!a.types.find(t => t === 'premise'));
        let streetNumber = addressComponents.find(a => !!a.types.find(t => t === 'street_number')).long_name;
        address.buildingNumber = premise != null ? `${premise.long_name}/${streetNumber}` : streetNumber;

        let postCode = addressComponents.find(a => !!a.types.find(t => t === 'postal_code')).long_name;
        address.postCode = postCode;

        let city = addressComponents.find(a => !!a.types.find(t => t === 'locality')).long_name;
        address.city = city;

        let street = addressComponents.find(a => !!a.types.find(t => t === 'route'));
        if (street != null) {
            address.street = street.long_name;
        }

        return address;
    }
}
