import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Client } from 'src/app/services/clients/client';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { ActivatedRoute } from '@angular/router';
import { ImageHelperService } from 'src/app/services/image-helper.service';
import { ClientsService, PictureDTO, CoordinatesDTO, IndividualPlansService } from 'src/app/api/generated';
import { ToastService } from 'src/app/services/toast.service';
import { GeneralHelperService } from 'src/app/services/general-helper.service';
import { IndividualPlanDTO } from 'src/app/api/generated/model/individualPlanDTO';
import { VisualHelperService } from 'src/app/services/visual-helper.service';
import { Platform } from '@ionic/angular';
import { Observable, fromEvent } from 'rxjs';
import { pluck } from 'rxjs/operators';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.page.html',
  styleUrls: ['./client-detail.page.scss'],
})
export class ClientDetailPage implements OnInit {
  @ViewChild('imageInput', { static: false }) imageInput: ElementRef;

  public client: Client;
  public individualPlans: IndividualPlanDTO[];
  public lat: number;
  public lng: number;

  constructor(
    private clientCache: ClientsCacheService,
    private activatedRoute: ActivatedRoute,
    private imageHelper: ImageHelperService,
    private plansService: IndividualPlansService,
    private clientsService: ClientsService,
    private toaster: ToastService,
    public generalHelper: GeneralHelperService,
    public visualHelper: VisualHelperService,
    private platform: Platform
  ) { }

  async ngOnInit() {
    await this.clientCache.loaded;
    let id = this.activatedRoute.snapshot.paramMap.get('id');
    this.client = this.clientCache.getClientById(id);

    this.getIndividualPlans();

    this.lat = +this.client.address.coordinates.latitude;
    this.lng = +this.client.address.coordinates.longitude;
  }

  private async getIndividualPlans() {
    let plans = await this.plansService.apiIndividualPlansGet(this.client.id);

    this.individualPlans = plans;
  }

  public async changeClientProfilePicture() {
    if (this.platform.is('capacitor') === false) {
      this.imageInput.nativeElement.click();
      return;
    }

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

  // https://stackoverflow.com/questions/39272970/angular-2-encode-image-to-base64
  public processFile(ev: any) {
    let files = ev.target.files;
    let file: File = files[0];

    if (files && file) {
      let reader = new FileReader();

      this.imageToBase64(reader, file)
        .subscribe(base64image => {

          if (base64image != null) {
            let dto: PictureDTO = {
              pictureUri: base64image
            };

            this.clientsService.apiClientsChangeProfilePicturePost(this.client.id, dto)
              .then(async () => {
                let client = await this.clientCache.reloadSingleClient(this.client.id);
                this.client = client;
              })
              .catch(() => this.toaster.showErrorToast('Changing image failed'));
          }
        });
    }
  }

  private imageToBase64(fileReader: FileReader, fileToRead: File): Observable<string> {
    fileReader.readAsDataURL(fileToRead);
    return fromEvent(fileReader, 'load').pipe(pluck('currentTarget', 'result'));
  }
}
