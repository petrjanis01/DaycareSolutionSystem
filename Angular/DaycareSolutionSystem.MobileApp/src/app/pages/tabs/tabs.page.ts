import { Component, OnInit, ViewChild, AfterContentInit, AfterViewInit } from '@angular/core';
import { ClientsCacheService } from 'src/app/services/clients/clients-cache.service';
import { VisualHelperService } from 'src/app/services/visual-helper.service';
import { IonTabs } from '@ionic/angular';

@Component({
  selector: 'app-tabs',
  templateUrl: 'tabs.page.html',
  styleUrls: ['tabs.page.scss']
})
export class TabsPage implements AfterViewInit {
  @ViewChild(IonTabs, { static: false }) tabs: IonTabs;

  constructor(private clientsCache: ClientsCacheService, public visualHelper: VisualHelperService) { }

  ngAfterViewInit() {
    this.clientsCache.loadClientsCache();
    this.visualHelper.tabs = this.tabs;
  }


}
