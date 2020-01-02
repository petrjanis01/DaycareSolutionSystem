import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { RegisteredActionsClientDTO } from 'src/app/api/generated';
import { ClientDTO } from 'src/app/api/generated';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';

@Component({
  selector: 'dss-client-action',
  templateUrl: './client-action.component.html',
  styleUrls: ['./client-action.component.scss'],
})
export class ClientActionComponent implements OnChanges {
  @Input() clientActions: RegisteredActionsClientDTO;

  public client: Client;

  constructor(private clientCache: ClientsCacheService) { }

  async ngOnChanges(changes: SimpleChanges) {
    let change = changes.clientActions;

    if (change && change.currentValue !== change.previousValue) {
      let clients = this.clientCache.clients;
      let clientId = this.clientActions.clientId;

      let client = clients.find(c => c.id === clientId);
      this.client = client;
    }
  }

  public getDafaultImageIfNotExists(img: string): string {
    if (img == null) {
      return './../../../../assets/img/user-anonymous.png';
    }

    return img;
  }
}
