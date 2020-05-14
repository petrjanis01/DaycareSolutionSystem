import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import { ClientsService, ClientDTO, AddressDTO, CoordinatesDTO } from 'src/app/api/generated';
import { Client } from './client';
import { ImageHelperService } from '../image-helper.service';
import { GeolocationHelperService } from '../geolocation/geolocation-helper-service';
import { Address } from '../geolocation/address';
import { GeneralHelperService } from '../general-helper.service';
import { ToastService } from '../toast.service';

@Injectable({ providedIn: 'root' })
export class ClientsCacheService {
    public clients: Client[];
    private alreadyLoaded: any;
    private defaultProfilePicture: string;

    public loaded = new Promise<any>(async (x) => {
        this.alreadyLoaded = x;
    });

    constructor(
        private clientsService: ClientsService,
        private loadingController: LoadingController,
        private imageHelper: ImageHelperService,
        private geolocationHelper: GeolocationHelperService,
        private helper: GeneralHelperService,
        private toasterService: ToastService
    ) { }

    public async loadClientsCache() {
        this.loaded = new Promise<any>(async (x) => {
            this.alreadyLoaded = x;
        });

        let loading = await this.loadingController.create({
            message: 'Loading...',
        });
        await loading.present();

        try {
            await this.loadDefaulProfilePicture();
            console.log('location');
            let deviceCords = await this.geolocationHelper.getCurrentLocation();
            console.log('loc done');
            console.log(deviceCords);
            let clientDtos = await this.clientsService.apiClientsGet(null);
            console.log('clients done');
            let clients = new Array<Client>();

            for (let dto of clientDtos) {
                let client = await this.mapDtoToClient(dto);
                await this.getCoordinatesIfNeeded(client);
                await this.getAddressIfNeeded(client);

                if (deviceCords != null) {
                    let distance = this.geolocationHelper.getDistnaceBetween2Coordinates(deviceCords, client.address.coordinates);
                    client.distanceFromDevice = Math.round(distance);
                }

                clients.push(client);
            }
            this.clients = clients;

        } catch {
            this.toasterService.showErrorToast('Failed to load clients');
        } finally {
            loading.dismiss();
            this.alreadyLoaded();
        }

        let cords = await this.geolocationHelper.getCurrentLocation();
        if (cords == null) {
            return;
        }

        setInterval(() => this.recalculateDeviceClientDistance(), 60 * 1000);
    }

    private async recalculateDeviceClientDistance() {
        let deviceCords = await this.geolocationHelper.getCurrentLocation();
        for (let client of this.clients) {
            let distance = this.geolocationHelper.getDistnaceBetween2Coordinates(deviceCords, client.address.coordinates);
            client.distanceFromDevice = Math.round(distance);
        }
    }

    public async reloadSingleClient(id: string): Promise<Client> {
        let dto = await this.clientsService.apiClientsSingleClientGet(id);
        let client = await this.mapDtoToClient(dto);

        let index = this.clients.findIndex(cl => cl.id === client.id);
        this.clients[index] = client;

        return client;
    }

    private async loadDefaulProfilePicture() {
        let img = await this.imageHelper.fileToBase64(this.helper.getAnonymousImgUrlFormatted());

        this.defaultProfilePicture = img;
    }

    public async mapDtoToClient(dto: ClientDTO): Promise<Client> {
        let client = new Client();
        client.id = dto.id;
        client.firstName = dto.firstName;
        client.surname = dto.surname;
        client.fullName = dto.fullName;
        client.birthDate = dto.birthDate;
        client.gender = dto.gender;
        client.profilePicture = dto.profilePicture.pictureUri != null ? dto.profilePicture.pictureUri : this.defaultProfilePicture;
        client.address = new Address(dto.address);
        client.distanceFromDevice = null;
        client.phoneNumber = dto.phoneNumber;
        client.email = dto.email;

        return client;
    }

    public getClientById(id: string): Client {
        let client = this.clients.find(c => c.id === id);
        return client;
    }

    public async getAddressIfNeeded(client: Client) {
        let address = new Address(client.address);

        let isAddressComplete = address.city == null || address.buildingNumber == null || address.postCode == null;
        if (isAddressComplete) {
            let addressCalculated = await (await this.geolocationHelper.getAddressFromgGpsCoordinates(address.coordinates));
            client.address.buildingNumber = addressCalculated.buildingNumber;
            client.address.street = addressCalculated.street;
            client.address.postCode = addressCalculated.postCode;
            client.address.city = addressCalculated.city;
        }
    }

    public async getCoordinatesIfNeeded(client: Client) {
        let address = new Address(client.address);

        if (address.coordinates == null) {
            let coordinates = await (await this.geolocationHelper.getGpsCoordinatesFromAddress(address));
            client.address.coordinates = coordinates;
        }
    }
}
