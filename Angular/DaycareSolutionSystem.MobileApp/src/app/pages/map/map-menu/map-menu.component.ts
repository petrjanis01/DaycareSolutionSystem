import { Component, Input, Output, EventEmitter, AfterContentInit } from '@angular/core';
import { PopoverController } from '@ionic/angular';
import { Client } from 'src/app/services/clients/client';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';

@Component({
  selector: 'app-map-menu',
  templateUrl: './map-menu.component.html',
  styleUrls: ['./map-menu.component.scss'],
})
export class MapMenuComponent {
  @Output() onallClientsChanged = new EventEmitter<boolean>();
  @Input() allClientsOnMap: boolean;
  @Input() hasDeviceLocation: boolean;
  @Input() nearbyClients: Client[];


  constructor(private popover: PopoverController) { }

  // https://www.freakyjolly.com/ionic-4-how-to-use-ionic-modal-popovers-and-pass-data-and-receive-response/
  public onAllClientsChange(ev: any) {
    this.popover.dismiss({
      allClientsOnMap: this.allClientsOnMap
    });
  }
}
