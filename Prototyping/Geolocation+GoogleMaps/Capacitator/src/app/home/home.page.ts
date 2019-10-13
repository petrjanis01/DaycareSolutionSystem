import { Component, ViewChild } from '@angular/core';
import { GoogleMapComponent } from '../shared/google-map/google-map.component';
import { Plugins, GeolocationPosition } from '@capacitor/core';

const { Geolocation } = Plugins;

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild(GoogleMapComponent, { static: true }) mapComponent: GoogleMapComponent;

  currentLocation: GeolocationPosition;

  constructor() {
    Geolocation.getCurrentPosition().then((position) => {
      this.currentLocation = position;
    });
  }

  testMarker() {
    let center = this.mapComponent.map.getCenter();
    this.mapComponent.addMarker(center.lat(), center.lng());
  }
}
