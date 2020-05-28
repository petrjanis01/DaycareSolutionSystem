import { Component, OnInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';
import { VisualHelperService } from 'src/app/services/visual-helper.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.page.html',
  styleUrls: ['./clients.page.scss'],
})
export class ClientsPage implements OnInit {
  public clients: Client[];

  constructor(private clientCache: ClientsCacheService, public visualHelper: VisualHelperService) { }

  async ngOnInit() {
    await this.clientCache.loaded;

    this.clients = this.clientCache.clients;
  }

}
