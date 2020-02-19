import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import { ClientsService, ClientDTO, AddressDTO } from 'src/app/api/generated';
import { Client } from './client';
import { ImageHelperService } from '../image-helper.service';
import { GeolocationHelperService } from '../geolocation/geolocation-helper-service';
import { Address } from '../geolocation/address';

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
        private geolocationHelper: GeolocationHelperService
    ) { }

    public async loadClientsCache() {
        this.loaded = new Promise<any>(async (x) => {
            this.alreadyLoaded = x;
        });

        let loading = await this.loadingController.create({
            message: 'Loading...',
        });
        await loading.present();

        await this.loadDefaulProfilePicture();

        await this.clientsService.apiClientsGet(null).then(dtos => {
            this.clients = new Array<Client>();
            dtos.forEach(async dto => {
                let client = await this.mapDtoToClient(dto);
                this.clients.push(client);
            });
        }).finally(() => {
            loading.dismiss();
            this.alreadyLoaded();
        });

        this.loadDistancesFromClients();
    }

    public async reloadSingleClient(id: string): Promise<Client> {
        let dto = await this.clientsService.apiClientsSingleClientGet(id);
        let client = await this.mapDtoToClient(dto);

        let index = this.clients.findIndex(cl => cl.id === client.id);
        this.clients[index] = client;

        return client;
    }

    private async loadDefaulProfilePicture() {
        let img = await this.imageHelper.fileToBase64('./../../../../assets/img/user-anonymous.png');

        this.defaultProfilePicture = img;
    }

    private async mapDtoToClient(dto: ClientDTO): Promise<Client> {
        let client = new Client();
        client.id = dto.id;
        client.firstName = dto.firstName;
        client.surname = dto.surname;
        client.fullName = dto.fullName;
        client.birthDate = dto.birthDate;
        client.gender = dto.gender;
        client.profilePicture = dto.profilePicture.pictureUri != null ? dto.profilePicture.pictureUri : this.defaultProfilePicture;
        client.address = dto.address;
        client.distanceFromDevice = null;

        return client;
    }

    public async getClientById(id: string): Promise<Client> {
        await this.loaded;

        let client = this.clients.find(c => c.id === id);
        return client;
    }

    private loadDistancesFromClients() {
        // TODO Use google map api on each client and find their distance
    }
}
