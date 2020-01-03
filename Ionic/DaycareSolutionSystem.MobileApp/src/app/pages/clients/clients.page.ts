import { Component, OnInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.page.html',
  styleUrls: ['./clients.page.scss'],
})
export class ClientsPage implements OnInit {
  public clients: Client[];

  constructor(private clientCache: ClientsCacheService) { }

  async ngOnInit() {
    await this.clientCache.loaded;

    this.clients = this.clientCache.clients;
  }

}
