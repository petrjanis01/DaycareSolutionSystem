import { Component, Input, EventEmitter, OnInit, Output } from '@angular/core';
import { RegisteredActionsClientDTO } from 'src/app/api/generated';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';
import { ModalController } from '@ionic/angular';
import { ClientActionDetailPage } from '../client-action-detail/client-action-detail.page';

@Component({
  selector: 'client-actions-overview',
  templateUrl: './client-actions-overview.component.html',
  styleUrls: ['./client-actions-overview.component.scss'],
})
export class ClientActionsOverviewComponent implements OnInit {
  @Input() clientActions: RegisteredActionsClientDTO;
  @Output() detailModalClosed = new EventEmitter();

  public client: Client;

  constructor(private clientCache: ClientsCacheService, private modalController: ModalController) { }

  async ngOnInit() {
    let clientId = this.clientActions.clientId;
    this.client = await this.clientCache.getClientById(clientId);
  }

  public calculateEstimatedEndTime(date: Date, minutes: number): Date {
    return new Date(date.getTime() + minutes * 60000);
  }

  public async openDetailModal(actionId: string) {
    let actionToPass = this.clientActions.registeredActions.find(a => a.id === actionId);

    let modal = await this.modalController.create({
      component: ClientActionDetailPage,
      componentProps: {
        action: actionToPass
      }
    });

    await modal.present();

    // TODO reloading same ammount of data when going back from modal and croll to same location
    // for now it work by reseting instance of infinite scroll and going back to top
    modal.onDidDismiss().then(() => this.detailModalClosed.emit());
  }
}
