import { Component, OnInit } from '@angular/core';
import { Client } from 'src/app/services/clients/client';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { ActivatedRoute } from '@angular/router';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { ClientsService, PictureDTO } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.page.html',
  styleUrls: ['./client-detail.page.scss'],
})
export class ClientDetailPage implements OnInit {
  public client: Client;

  constructor(
    private clientCache: ClientsCacheService,
    private activatedRoute: ActivatedRoute,
    private imageHelper: ImageHelperService,
    private clientsService: ClientsService,
    private toaster: ToastService,
    public generalHelper: GeneralHelperService
  ) { }

  async ngOnInit() {
    let id = this.activatedRoute.snapshot.paramMap.get('id');
    this.client = await this.clientCache.getClientById(id);
  }

  public async changeClientProfilePicture() {
    let img = await this.imageHelper.getImageAsBase64FromDevice();

    if (img != null) {
      let dto: PictureDTO = {
        pictureUri: img
      };

      this.clientsService.apiClientsChangeProfilePicturePost(this.client.id, dto)
        .then(async () => {
          let client = await this.clientCache.reloadSingleClient(this.client.id);
          this.client = client;
        })
        .catch(() => this.toaster.showErrorToast('Changing image failed'));
    }
  }
}
