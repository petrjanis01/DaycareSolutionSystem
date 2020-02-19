import { Component, OnInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { Client } from 'src/app/services/clients/client';

@Component({
  selector: 'app-map',
  templateUrl: './map.page.html',
  styleUrls: ['./map.page.scss'],
})
export class MapPage implements OnInit {
  clients: Client[];

  constructor(private cache: ClientsCacheService) { }

  async ngOnInit() {
    await this.cache.loaded;
    this.clients = this.cache.clients;
  }

}
